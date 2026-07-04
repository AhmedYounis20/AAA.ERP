using Accounting.Contracts.Integration;
using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory;
using ERP.Domain.Models.Entities.Inventory;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using ERP.Domain.Models.Entities.Inventory.Variants;

namespace ERP.Infrastracture.Services.Inventory;

public class InventoryTransferService : BaseService<InventoryTransfer, InventoryTransferCreateCommand, InventoryTransferUpdateCommand>, IInventoryTransferService
{
    private readonly IInventoryTransferRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _dbContext;
    private readonly IAccountingIntegrationService _accountingIntegration;

    public InventoryTransferService(
        IInventoryTransferRepository repository,
        IUnitOfWork unitOfWork,
        ApplicationDbContext dbContext,
        IAccountingIntegrationService accountingIntegration) : base(repository)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _accountingIntegration = accountingIntegration;
    }

    public override async Task<ApiResponse<InventoryTransfer>> Create(InventoryTransferCreateCommand command, bool isValidate = true)
    {
        var transfer = new InventoryTransfer
        {
            SourceBranchId = command.SourceBranchId,
            DestinationBranchId = command.DestinationBranchId,
            Notes = command.Notes,
            TransferType = command.TransferType,
            Status = command.TransferType == InventoryTransferType.Direct ? InventoryTransferStatus.Approved : InventoryTransferStatus.Pending,
            ApprovedAt = command.TransferType == InventoryTransferType.Direct ? DateTime.Now : null,
            Items = command.Items.Select(i => new InventoryTransferItem
            {
                VariantId = i.VariantId,
                PackingUnitId = i.PackingUnitId,
                Quantity = i.Quantity,
            }).ToList()
        };

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _repository.Add(transfer);

            var sourceBranchResult = await _accountingIntegration.GetBranchAccountAsync(transfer.SourceBranchId);
            var destBranchResult = await _accountingIntegration.GetBranchAccountAsync(transfer.DestinationBranchId);
            if (!sourceBranchResult.IsSuccess || !destBranchResult.IsSuccess ||
                sourceBranchResult.Result?.ChartOfAccountId is null ||
                destBranchResult.Result?.ChartOfAccountId is null)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer>
                {
                    IsSuccess = false,
                    Errors = [new MessageTemplate { MessageKey = "BranchOrBranchAccountNotFound" }]
                };
            }

            var sourceAccountId = sourceBranchResult.Result.ChartOfAccountId.Value;
            var destAccountId = command.TransferType == InventoryTransferType.Direct
                ? destBranchResult.Result.ChartOfAccountId.Value
                : Guid.Parse(SD.StockUnderTransfer_Branch);
            var destinationBranchId = command.TransferType == InventoryTransferType.Direct
                ? command.DestinationBranchId
                : destAccountId;

            var moveResult = await MoveItems(command.SourceBranchId, destinationBranchId, transfer.Items);
            if (!moveResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer>
                {
                    IsSuccess = moveResult.IsSuccess,
                    Errors = moveResult.Errors,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var entryResult = await PostTransferGlEntryAsync(
                transfer.Items,
                sourceBranchResult.Result.BranchId,
                sourceAccountId,
                destAccountId);

            if (!entryResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer>
                {
                    IsSuccess = false,
                    StatusCode = entryResult.StatusCode,
                    Errors = entryResult.Errors
                };
            }

            await _unitOfWork.CommitAsync();
            return new ApiResponse<InventoryTransfer> { IsSuccess = true, Result = transfer };
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = false,
                Result = transfer,
                Errors = [new MessageTemplate { MessageKey = "OperationFaild" }]
            };
        }
    }

    private async Task<ApiResponse> MoveItems(Guid sourceId, Guid destinationId, List<InventoryTransferItem> items)
    {
        foreach (var item in items)
        {
            var variantUsedPackingUnit = await _dbContext.Set<VariantPackingUnit>().AsNoTracking()
                .FirstOrDefaultAsync(e => e.VariantId == item.VariantId && item.PackingUnitId == e.PackingUnitId);
            if (variantUsedPackingUnit == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Errors = [new MessageTemplate { MessageKey = "FaildToGetUsedPackingUnit" }],
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            decimal quantity = item.Quantity / variantUsedPackingUnit.PartsCount;

            var sourceStock = await _dbContext.Set<StockBalance>().AsNoTracking()
                .FirstOrDefaultAsync(e => e.VariantId == item.VariantId && e.BranchId == sourceId);
            if (sourceStock == null || sourceStock.CurrentBalance < quantity)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Errors = [new MessageTemplate { MessageKey = "VariantWithNowStockBalance" }],
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            sourceStock.CurrentBalance -= quantity;
            sourceStock.TotalCost -= quantity * sourceStock.UnitCost;
            sourceStock.ModifiedAt = DateTime.Now;
            await _unitOfWork.StockBalanceRepository.Update(sourceStock);

            var destinationBranchResult = await _accountingIntegration.GetBranchAccountAsync(destinationId);
            var destinationStock = await _dbContext.Set<StockBalance>().AsNoTracking()
                .FirstOrDefaultAsync(e => e.VariantId == item.VariantId && e.BranchId == destinationId);

            if (destinationStock == null)
            {
                if (!destinationBranchResult.IsSuccess)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        Errors = [new MessageTemplate { MessageKey = "DistinationBranchNotFound" }],
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                var defaultPackingUnit = await _dbContext.Set<VariantPackingUnit>().AsNoTracking()
                    .FirstOrDefaultAsync(e => e.IsDefaultPackingUnit && e.VariantId == item.VariantId);
                var usedPackingUnit = await _dbContext.Set<VariantPackingUnit>().AsNoTracking()
                    .FirstOrDefaultAsync(e => e.PackingUnitId == item.PackingUnitId && e.VariantId == item.VariantId);
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
                await _unitOfWork.StockBalanceRepository.Add(destinationStock);
            }
            else
            {
                var defaultPackingUnit = await _dbContext.Set<VariantPackingUnit>().AsNoTracking()
                    .FirstOrDefaultAsync(e => e.IsDefaultPackingUnit && e.VariantId == item.VariantId);

                destinationStock.CurrentBalance += quantity;
                destinationStock.ModifiedAt = DateTime.Now;
                destinationStock.UnitCost = (destinationStock.TotalCost + ((defaultPackingUnit?.AverageCostPrice ?? 0) * quantity)) / destinationStock.CurrentBalance;
                destinationStock.TotalCost = destinationStock.UnitCost * destinationStock.CurrentBalance;
                await _unitOfWork.StockBalanceRepository.Update(destinationStock);
            }
        }

        return new ApiResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK
        };
    }

    private async Task<ApiResponse<Guid>> PostTransferGlEntryAsync(
        List<InventoryTransferItem> items,
        Guid branchId,
        Guid sourceAccountId,
        Guid destAccountId)
    {
        var now = DateTime.UtcNow;
        var periodResult = await _accountingIntegration.GetOpenPeriodForDateAsync(now);
        if (!periodResult.IsSuccess || periodResult.Result is null)
        {
            await _unitOfWork.RollbackAsync();
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
            var variantUsedPackingUnit = await _dbContext.Set<VariantPackingUnit>().AsNoTracking()
                .FirstOrDefaultAsync(e => e.PackingUnitId == item.PackingUnitId && e.VariantId == item.VariantId);
            totalPrice += item.Quantity * (variantUsedPackingUnit?.AverageCostPrice ?? 0);
        }

        return await _accountingIntegration.PostInventoryTransferEntryAsync(
            new InventoryTransferPostingRequest(
                branchId,
                destAccountId,
                sourceAccountId,
                now,
                periodResult.Result.Id,
                totalPrice));
    }

    public override async Task<ApiResponse<InventoryTransfer>> Update(InventoryTransferUpdateCommand command, bool isValidate = true)
    {
        var transfer = await _repository.GetWithDetails(command.Id);
        if (transfer == null)
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, Errors = [new MessageTemplate { MessageKey = "TransferNotFound" }] };

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

        _repository.Update(transfer);
        return new ApiResponse<InventoryTransfer> { IsSuccess = true, Result = transfer };
    }

    public async Task<ApiResponse<InventoryTransfer>> GetWithDetails(Guid id)
    {
        var transfer = await _repository.GetWithDetails(id);
        if (transfer == null)
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, Errors = [new MessageTemplate { MessageKey = "TransferNotFound" }] };
        return new ApiResponse<InventoryTransfer> { IsSuccess = true, Result = transfer };
    }

    public async Task<ApiResponse<IEnumerable<InventoryTransfer>>> GetByStatus(InventoryTransferStatus status)
    {
        var transfers = await _repository.GetByStatus(status);
        return new ApiResponse<IEnumerable<InventoryTransfer>> { IsSuccess = true, Result = transfers };
    }

    public async Task<ApiResponse<InventoryTransfer>> ApproveTransfer(Guid id)
    {
        var transfer = await _repository.Get(id);
        if (transfer == null)
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, Errors = [new MessageTemplate { MessageKey = "TransferNotFound" }] };
        if (transfer.Status != InventoryTransferStatus.Pending)
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, Errors = [new MessageTemplate { MessageKey = "TransferIsNotPending" }] };

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            transfer.Status = InventoryTransferStatus.Approved;
            transfer.ApprovedAt = DateTime.UtcNow;
            await _repository.Update(transfer);

            var destBranchResult = await _accountingIntegration.GetBranchAccountAsync(transfer.DestinationBranchId);
            if (!destBranchResult.IsSuccess || destBranchResult.Result?.ChartOfAccountId is null)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer>
                {
                    IsSuccess = false,
                    Errors = [new MessageTemplate { MessageKey = "BranchOrBranchAccountNotFound" }]
                };
            }

            var sourceAccountId = Guid.Parse(SD.StockUnderTransfer_Branch);
            var destAccountId = destBranchResult.Result.ChartOfAccountId.Value;
            var sourceBranchId = Guid.Parse(SD.StockUnderTransfer_Branch);
            var items = await _dbContext.Set<InventoryTransferItem>()
                .Where(e => e.InventoryTransferId == transfer.Id)
                .ToListAsync();

            var moveResult = await MoveItems(sourceBranchId, destBranchResult.Result.BranchId, items);
            if (!moveResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer>
                {
                    IsSuccess = moveResult.IsSuccess,
                    Errors = moveResult.Errors,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var entryResult = await PostTransferGlEntryAsync(items, destBranchResult.Result.BranchId, sourceAccountId, destAccountId);
            if (!entryResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer> { IsSuccess = false, Errors = entryResult.Errors };
            }

            await _unitOfWork.CommitAsync();
            return new ApiResponse<InventoryTransfer> { IsSuccess = true, Result = transfer };
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError
            };
        }
    }

    public async Task<ApiResponse<InventoryTransfer>> RejectTransfer(Guid id, Guid approverId, string? reason)
    {
        var transfer = await _repository.GetWithDetails(id);
        if (transfer == null)
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, Errors = [new MessageTemplate { MessageKey = "TransferNotFound" }] };
        if (transfer.Status != InventoryTransferStatus.Pending)
            return new ApiResponse<InventoryTransfer> { IsSuccess = false, Errors = [new MessageTemplate { MessageKey = "TransferIsNotPending" }] };

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            transfer.Status = InventoryTransferStatus.Rejected;
            transfer.ApprovedAt = DateTime.UtcNow;
            await _repository.Update(transfer);

            var sourceBranchResult = await _accountingIntegration.GetBranchAccountAsync(transfer.SourceBranchId);
            if (!sourceBranchResult.IsSuccess || sourceBranchResult.Result?.ChartOfAccountId is null)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer>
                {
                    IsSuccess = false,
                    Errors = [new MessageTemplate { MessageKey = "BranchOrBranchAccountNotFound" }]
                };
            }

            var sourceAccountId = Guid.Parse(SD.StockUnderTransfer_Branch);
            var destAccountId = sourceBranchResult.Result.ChartOfAccountId.Value;
            var stockUnderTransferBranchId = Guid.Parse(SD.StockUnderTransfer_Branch);
            var items = await _dbContext.Set<InventoryTransferItem>()
                .Where(e => e.InventoryTransferId == transfer.Id)
                .ToListAsync();

            var moveResult = await MoveItems(stockUnderTransferBranchId, transfer.SourceBranchId, items);
            if (!moveResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer>
                {
                    IsSuccess = moveResult.IsSuccess,
                    Errors = moveResult.Errors,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var entryResult = await PostTransferGlEntryAsync(
                items,
                stockUnderTransferBranchId,
                sourceAccountId,
                destAccountId);

            if (!entryResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return new ApiResponse<InventoryTransfer> { IsSuccess = false, Errors = entryResult.Errors };
            }

            await _unitOfWork.CommitAsync();
            return new ApiResponse<InventoryTransfer> { IsSuccess = true, Result = transfer };
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            return new ApiResponse<InventoryTransfer>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError
            };
        }
    }
}
