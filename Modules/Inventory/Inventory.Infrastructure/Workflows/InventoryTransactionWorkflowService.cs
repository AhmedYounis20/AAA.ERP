using Accounting.Contracts.Integration;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.Constants;
using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.InventoryTransactions;

namespace Inventory.Infrastructure.Workflows;

/// <summary>
/// Inventory transaction write orchestration (stock balance, GL posting, transactional coordinator).
/// </summary>
public class InventoryTransactionWorkflowService(
    IInventoryTransactionRepository repository,
    IStockBalanceService stockBalanceService,
    ITransactionCoordinator transactionCoordinator,
    IAccountingIntegrationService accountingIntegration) : IInventoryTransactionWorkflowService
{
    public Task<ApiResponse<InventoryTransaction>> Create(InventoryTransactionCreateCommand command, bool isValidate = true)
        => transactionCoordinator.ExecuteAsync(ct => CreateWithinTransactionAsync(command, isValidate, ct));

    public async Task<ApiResponse<InventoryTransaction>> Update(BaseUpdateCommand<InventoryTransaction> command)
    {
        try
        {
            var validationResult = await ValidateUpdate(command);
            if (!validationResult.isValid)
            {
                return new ApiResponse<InventoryTransaction>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            var entity = validationResult.entity!;
            var updated = command.Adapt<InventoryTransaction>();
            updated.ModifiedAt = DateTime.Now;
            await repository.Update(updated);

            return new ApiResponse<InventoryTransaction>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = updated
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<InventoryTransaction>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<InventoryTransaction>> Delete(Guid id)
    {
        try
        {
            var validationResult = await ValidateDelete(id);
            if (!validationResult.isValid)
            {
                return new ApiResponse<InventoryTransaction>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            var entity = validationResult.entity;
            if (entity is not null)
                await repository.Delete(entity);

            return new ApiResponse<InventoryTransaction>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<InventoryTransaction>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    private async Task<ApiResponse<InventoryTransaction>> CreateWithinTransactionAsync(
        InventoryTransactionCreateCommand command,
        bool isValidate,
        CancellationToken cancellationToken)
    {
        if (isValidate)
        {
            var validationResult = await ValidateCreate(command);
            if (!validationResult.isValid)
            {
                return new ApiResponse<InventoryTransaction>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }
        }

        var periodResult = await accountingIntegration.GetOpenPeriodForDateAsync(command.TransactionDate);
        if (!periodResult.IsSuccess || periodResult.Result is null)
        {
            return new ApiResponse<InventoryTransaction>
            {
                IsSuccess = false,
                StatusCode = periodResult.StatusCode,
                Errors = periodResult.Errors
            };
        }

        var financialPeriod = periodResult.Result;

        var lastTransactionNumber = repository.GetQuery()
            .Where(e => e.FinancialPeriodId == financialPeriod.Id)
            .ToList()
            .OrderByDescending(e => BigInteger.Parse(e.TransactionNumber))
            .Select(e => BigInteger.Parse(e.TransactionNumber))
            .FirstOrDefault();

        var transactionNumber = lastTransactionNumber == 0
            ? "1"
            : (lastTransactionNumber + 1).ToString();

        var transaction = command.Adapt<InventoryTransaction>();
        transaction.TransactionNumber = transactionNumber;
        transaction.FinancialPeriodId = financialPeriod.Id;
        transaction.Items = command.Items.Select(item => new InventoryTransactionItem
        {
            VariantId = item.VariantId,
            PackingUnitId = item.PackingUnitId,
            Quantity = item.Quantity,
            TotalCost = item.TotalCost
        }).ToList();

        if (command.TransactionType == InventoryTransactionType.Issue)
        {
            foreach (var item in command.Items)
            {
                var stockBalanceResp =
                    await stockBalanceService.GetCurrentBalance(item.VariantId, item.PackingUnitId, command.BranchId);
                var currentBalance = stockBalanceResp.Result;
                if (currentBalance < item.Quantity)
                {
                    return new ApiResponse<InventoryTransaction>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = [new MessageTemplate { MessageKey = ErrorMessages.InsufficientStockBalance }]
                    };
                }
            }
        }

        var createdTransaction = await repository.Add(transaction);

        foreach (var item in command.Items)
        {
            var unitCost = item.TotalCost / item.Quantity;
            var isReceipt = command.TransactionType == InventoryTransactionType.Receipt;
            var updateResult = await stockBalanceService.UpdateStockBalance(
                item.VariantId,
                item.PackingUnitId,
                command.BranchId,
                item.Quantity,
                unitCost,
                isReceipt);
            if (!updateResult.IsSuccess)
                throw new Exception(ErrorMessages.FailedToAddStockBalance);
        }

        var totalAmount = command.Items.Sum(i => i.TotalCost);
        var glDirection = command.TransactionType == InventoryTransactionType.Receipt
            ? InventoryGlDirection.Receipt
            : InventoryGlDirection.Issue;

        var entryResult = await accountingIntegration.PostInventoryTransactionEntryAsync(
            new InventoryTransactionPostingRequest(
                createdTransaction.Id,
                command.BranchId,
                command.TransactionPartyId,
                command.TransactionDate,
                financialPeriod.Id,
                totalAmount,
                glDirection),
            cancellationToken);

        if (!entryResult.IsSuccess)
        {
            return new ApiResponse<InventoryTransaction>
            {
                IsSuccess = false,
                StatusCode = entryResult.StatusCode,
                Errors = entryResult.Errors
            };
        }

        return new ApiResponse<InventoryTransaction>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Result = createdTransaction
        };
    }

    private async Task<(bool isValid, List<string> errors)> ValidateCreate(InventoryTransactionCreateCommand command)
    {
        var validationResult = (isValid: true, errors: new List<string>());

        var periodResult = await accountingIntegration.GetOpenPeriodForDateAsync(command.TransactionDate);
        if (!periodResult.IsSuccess)
        {
            validationResult.isValid = false;
            validationResult.errors.Add(ErrorMessages.FinancialPeriodNotFound);
        }

        return validationResult;
    }

    private async Task<(bool isValid, List<string> errors, InventoryTransaction? entity)> ValidateUpdate(
        BaseUpdateCommand<InventoryTransaction> command)
    {
        var entity = await repository.Get(command.Id);
        if (entity == null)
        {
            return (false,
                [$"{nameof(InventoryTransaction)} with Id: {command.Id} not found"],
                null);
        }

        return (true, [], entity);
    }

    private async Task<(bool isValid, List<string> errors, InventoryTransaction? entity)> ValidateDelete(Guid id)
    {
        var entity = await repository.Get(id);
        if (entity is null)
            return (false, [ErrorMessages.RecordNotFound], null);

        return (true, [], entity);
    }
}
