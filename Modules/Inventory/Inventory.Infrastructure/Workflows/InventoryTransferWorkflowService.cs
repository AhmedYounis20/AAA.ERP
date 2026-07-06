using Accounting.Contracts.Integration;
using Inventory.Application.Commands.InventoryTransfers;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities;
using Inventory.Domain.Models.Entities.Sizes;
using Inventory.Domain.Models.Entities.Variants;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Infrastructure.Workflows;

/// <summary>
/// Inventory transfer write orchestration (stock moves, branch GL integration, approve/reject).
/// </summary>
public class InventoryTransferWorkflowService(
    IInventoryTransferRepository repository,
    IStockBalanceRepository stockBalanceRepository,
    ITransactionCoordinator transactionCoordinator,
    IApplicationDbContext dbContext,
    IAccountingIntegrationService accountingIntegration) : IInventoryTransferWorkflowService
{
    public Task<ApiResponse<InventoryTransfer>> Create(InventoryTransferCreateCommand command)
        => transactionCoordinator.ExecuteAsync(ct => CreateWithinTransactionAsync(command, ct));

    public async Task<ApiResponse<InventoryTransfer>> Update(InventoryTransferUpdateCommand command)
    {
        var transfer = await repository.GetWithDetails(command.Id);
        if (transfer == null)
        {
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.TransferNotFound }]
            };
        }

        transfer.SourceBranchId = command.SourceBranchId;
        transfer.DestinationBranchId = command.DestinationBranchId;
        transfer.Notes = command.Notes;
        transfer.Items.Clear();
        foreach (var i in command.Items)
        {
            transfer.Items.Add(new InventoryTransferItem
            {
                VariantId = i.VariantId,
                PackingUnitId = i.PackingUnitId,
                Quantity = i.Quantity,
            });
        }

        repository.Update(transfer);
        return new ApiResponse<InventoryTransfer> { IsSuccess = true, Result = transfer };
    }

    public Task<ApiResponse<InventoryTransfer>> ApproveTransfer(Guid id)
        => transactionCoordinator.ExecuteAsync(ct => ApproveTransferWithinTransactionAsync(id, ct));

    public Task<ApiResponse<InventoryTransfer>> RejectTransfer(Guid id, Guid approverId, string? reason)
        => transactionCoordinator.ExecuteAsync(ct => RejectTransferWithinTransactionAsync(id, approverId, reason, ct));

    private async Task<ApiResponse<InventoryTransfer>> CreateWithinTransactionAsync(
        InventoryTransferCreateCommand command,
        CancellationToken cancellationToken)
    {
        var transfer = new InventoryTransfer
        {
            SourceBranchId = command.SourceBranchId,
            DestinationBranchId = command.DestinationBranchId,
            Notes = command.Notes,
            TransferType = command.TransferType,
            Status = command.TransferType == InventoryTransferType.Direct
                ? InventoryTransferStatus.Approved
                : InventoryTransferStatus.Pending,
            ApprovedAt = command.TransferType == InventoryTransferType.Direct ? DateTime.Now : null,
            Items = command.Items.Select(i => new InventoryTransferItem
            {
                VariantId = i.VariantId,
                PackingUnitId = i.PackingUnitId,
                Quantity = i.Quantity,
            }).ToList()
        };

        await repository.Add(transfer);

        var sourceBranchResult = await accountingIntegration.GetBranchAccountAsync(transfer.SourceBranchId, cancellationToken);
        var destBranchResult = await accountingIntegration.GetBranchAccountAsync(transfer.DestinationBranchId, cancellationToken);
        if (!sourceBranchResult.IsSuccess || !destBranchResult.IsSuccess ||
            sourceBranchResult.Result?.ChartOfAccountId is null ||
            destBranchResult.Result?.ChartOfAccountId is null)
        {
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.BranchOrBranchAccountNotFound }]
            };
        }

        var sourceAccountId = sourceBranchResult.Result.ChartOfAccountId.Value;
        var destAccountId = command.TransferType == InventoryTransferType.Direct
            ? destBranchResult.Result.ChartOfAccountId.Value
            : Guid.Parse(SD.StockUnderTransfer_Branch);
        var destinationBranchId = command.TransferType == InventoryTransferType.Direct
            ? command.DestinationBranchId
            : destAccountId;

        var moveResult = await MoveItems(command.SourceBranchId, destinationBranchId, transfer.Items, cancellationToken);
        if (!moveResult.IsSuccess)
        {
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = moveResult.IsSuccess,
                Errors = moveResult.Errors,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        var entryResult = await PostTransferGlEntryAsync(
            transfer.Id,
            command.TransferType == InventoryTransferType.Direct
                ? EntrySourceDocumentTypes.InventoryTransferComplete
                : EntrySourceDocumentTypes.InventoryTransferStage,
            transfer.Items,
            sourceBranchResult.Result.BranchId,
            sourceAccountId,
            destAccountId,
            cancellationToken);

        if (!entryResult.IsSuccess)
        {
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = false,
                StatusCode = entryResult.StatusCode,
                Errors = entryResult.Errors
            };
        }

        return new ApiResponse<InventoryTransfer> { IsSuccess = true, Result = transfer };
    }

    private async Task<ApiResponse<InventoryTransfer>> ApproveTransferWithinTransactionAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var transfer = await repository.Get(id);
        if (transfer == null)
        {
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.TransferNotFound }]
            };
        }

        if (transfer.Status != InventoryTransferStatus.Pending)
        {
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.TransferIsNotPending }]
            };
        }

        transfer.Status = InventoryTransferStatus.Approved;
        transfer.ApprovedAt = DateTime.UtcNow;
        await repository.Update(transfer);

        var destBranchResult = await accountingIntegration.GetBranchAccountAsync(transfer.DestinationBranchId, cancellationToken);
        if (!destBranchResult.IsSuccess || destBranchResult.Result?.ChartOfAccountId is null)
        {
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.BranchOrBranchAccountNotFound }]
            };
        }

        var sourceAccountId = Guid.Parse(SD.StockUnderTransfer_Branch);
        var destAccountId = destBranchResult.Result.ChartOfAccountId.Value;
        var sourceBranchId = Guid.Parse(SD.StockUnderTransfer_Branch);
        var items = await dbContext.Set<InventoryTransferItem>()
            .Where(e => e.InventoryTransferId == transfer.Id)
            .ToListAsync(cancellationToken);

        var moveResult = await MoveItems(sourceBranchId, destBranchResult.Result.BranchId, items, cancellationToken);
        if (!moveResult.IsSuccess)
        {
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = moveResult.IsSuccess,
                Errors = moveResult.Errors,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        var entryResult = await PostTransferGlEntryAsync(
            transfer.Id,
            EntrySourceDocumentTypes.InventoryTransferComplete,
            items,
            destBranchResult.Result.BranchId,
            sourceAccountId,
            destAccountId,
            cancellationToken);

        if (!entryResult.IsSuccess)
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, Errors = entryResult.Errors };

        return new ApiResponse<InventoryTransfer> { IsSuccess = true, Result = transfer };
    }

    private async Task<ApiResponse<InventoryTransfer>> RejectTransferWithinTransactionAsync(
        Guid id,
        Guid approverId,
        string? reason,
        CancellationToken cancellationToken)
    {
        var transfer = await repository.GetWithDetails(id);
        if (transfer == null)
        {
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.TransferNotFound }]
            };
        }

        if (transfer.Status != InventoryTransferStatus.Pending)
        {
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.TransferIsNotPending }]
            };
        }

        transfer.Status = InventoryTransferStatus.Rejected;
        transfer.ApprovedAt = DateTime.UtcNow;
        await repository.Update(transfer);

        var sourceBranchResult = await accountingIntegration.GetBranchAccountAsync(transfer.SourceBranchId, cancellationToken);
        if (!sourceBranchResult.IsSuccess || sourceBranchResult.Result?.ChartOfAccountId is null)
        {
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.BranchOrBranchAccountNotFound }]
            };
        }

        var sourceAccountId = Guid.Parse(SD.StockUnderTransfer_Branch);
        var destAccountId = sourceBranchResult.Result.ChartOfAccountId.Value;
        var stockUnderTransferBranchId = Guid.Parse(SD.StockUnderTransfer_Branch);
        var items = await dbContext.Set<InventoryTransferItem>()
            .Where(e => e.InventoryTransferId == transfer.Id)
            .ToListAsync(cancellationToken);

        var moveResult = await MoveItems(stockUnderTransferBranchId, transfer.SourceBranchId, items, cancellationToken);
        if (!moveResult.IsSuccess)
        {
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = moveResult.IsSuccess,
                Errors = moveResult.Errors,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        var entryResult = await PostTransferGlEntryAsync(
            transfer.Id,
            EntrySourceDocumentTypes.InventoryTransferReject,
            items,
            stockUnderTransferBranchId,
            sourceAccountId,
            destAccountId,
            cancellationToken);

        if (!entryResult.IsSuccess)
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, Errors = entryResult.Errors };

        return new ApiResponse<InventoryTransfer> { IsSuccess = true, Result = transfer };
    }

    private async Task<ApiResponse> MoveItems(
        Guid sourceId,
        Guid destinationId,
        List<InventoryTransferItem> items,
        CancellationToken cancellationToken = default)
    {
        foreach (var item in items)
        {
            var variantUsedPackingUnit = await dbContext.Set<VariantPackingUnit>().AsNoTracking()
                .FirstOrDefaultAsync(e => e.VariantId == item.VariantId && item.PackingUnitId == e.PackingUnitId, cancellationToken);
            if (variantUsedPackingUnit == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Errors = [new MessageTemplate { MessageKey = ErrorMessages.FaildToGetUsedPackingUnit }],
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            decimal quantity = item.Quantity / variantUsedPackingUnit.PartsCount;

            var sourceStock = await dbContext.Set<StockBalance>()
                .FirstOrDefaultAsync(e => e.VariantId == item.VariantId && e.BranchId == sourceId, cancellationToken);
            if (sourceStock == null || sourceStock.CurrentBalance < quantity)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Errors = [new MessageTemplate { MessageKey = ErrorMessages.VariantWithNowStockBalance }],
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            sourceStock.CurrentBalance -= quantity;
            sourceStock.TotalCost -= quantity * sourceStock.UnitCost;
            sourceStock.ModifiedAt = DateTime.Now;
            await stockBalanceRepository.Update(sourceStock);

            var destinationBranchResult = await accountingIntegration.GetBranchAccountAsync(destinationId, cancellationToken);
            var destinationStock = await dbContext.Set<StockBalance>()
                .FirstOrDefaultAsync(e => e.VariantId == item.VariantId && e.BranchId == destinationId, cancellationToken);

            if (destinationStock == null)
            {
                if (!destinationBranchResult.IsSuccess)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        Errors = [new MessageTemplate { MessageKey = ErrorMessages.DistinationBranchNotFound }],
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var defaultPackingUnit = await dbContext.Set<VariantPackingUnit>().AsNoTracking()
                    .FirstOrDefaultAsync(e => e.IsDefaultPackingUnit && e.VariantId == item.VariantId, cancellationToken);
                var usedPackingUnit = await dbContext.Set<VariantPackingUnit>().AsNoTracking()
                    .FirstOrDefaultAsync(e => e.PackingUnitId == item.PackingUnitId && e.VariantId == item.VariantId, cancellationToken);
                destinationStock = new StockBalance
                {
                    VariantId = item.VariantId,
                    BranchId = destinationId,
                    CreatedAt = DateTime.Now,
                    PackingUnitId = defaultPackingUnit?.PackingUnitId ?? Guid.Empty,
                    TotalCost = (usedPackingUnit?.AverageCostPrice ?? 0) * item.Quantity,
                    UnitCost = usedPackingUnit?.AverageCostPrice ?? 0,
                    ModifiedAt = DateTime.Now,
                    CurrentBalance = quantity
                };
                await stockBalanceRepository.Add(destinationStock);
            }
            else
            {
                var defaultPackingUnit = await dbContext.Set<VariantPackingUnit>().AsNoTracking()
                    .FirstOrDefaultAsync(e => e.IsDefaultPackingUnit && e.VariantId == item.VariantId, cancellationToken);

                destinationStock.CurrentBalance += quantity;
                destinationStock.ModifiedAt = DateTime.Now;
                destinationStock.UnitCost = (destinationStock.TotalCost + ((defaultPackingUnit?.AverageCostPrice ?? 0) * quantity)) / destinationStock.CurrentBalance;
                destinationStock.TotalCost = destinationStock.UnitCost * destinationStock.CurrentBalance;
                await stockBalanceRepository.Update(destinationStock);
            }
        }

        return new ApiResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };
    }

    private async Task<ApiResponse<Guid>> PostTransferGlEntryAsync(
        Guid transferId,
        string sourceDocumentType,
        List<InventoryTransferItem> items,
        Guid branchId,
        Guid sourceAccountId,
        Guid destAccountId,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var periodResult = await accountingIntegration.GetOpenPeriodForDateAsync(now, cancellationToken);
        if (!periodResult.IsSuccess || periodResult.Result is null)
        {
            return new ApiResponse<Guid>
            {
                IsSuccess = false,
                StatusCode = periodResult.StatusCode,
                Errors = periodResult.Errors
            };
        }

        decimal totalPrice = 0;
        foreach (var item in items)
        {
            var variantUsedPackingUnit = await dbContext.Set<VariantPackingUnit>().AsNoTracking()
                .FirstOrDefaultAsync(e => e.PackingUnitId == item.PackingUnitId && e.VariantId == item.VariantId, cancellationToken);
            totalPrice += item.Quantity * (variantUsedPackingUnit?.AverageCostPrice ?? 0);
        }

        return await accountingIntegration.PostInventoryTransferEntryAsync(
            new InventoryTransferPostingRequest(
                transferId,
                sourceDocumentType,
                branchId,
                destAccountId,
                sourceAccountId,
                now,
                periodResult.Result.Id,
                totalPrice),
            cancellationToken);
    }
}
