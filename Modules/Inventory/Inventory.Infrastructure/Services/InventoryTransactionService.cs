using Accounting.Contracts.Integration;
using Accounting.Application.Repositories;
using Inventory.Application.Repositories;
using ERP.Infrastructure.Services.BaseServices;
using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

namespace Inventory.Infrastructure.Services;

public class InventoryTransactionService : BaseService<InventoryTransaction, InventoryTransactionCreateCommand, InventoryTransactionUpdateCommand>, IInventoryTransactionService
{
    private readonly IInventoryTransactionRepository _repository;
    private readonly IStockBalanceService _stockBalanceService;
    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IAccountingIntegrationService _accountingIntegration;

    public InventoryTransactionService(
        IInventoryTransactionRepository repository,
        IStockBalanceService stockBalanceService,
        ITransactionCoordinator transactionCoordinator,
        IAccountingIntegrationService accountingIntegration) : base(repository)
    {
        _repository = repository;
        _stockBalanceService = stockBalanceService;
        _transactionCoordinator = transactionCoordinator;
        _accountingIntegration = accountingIntegration;
    }

    public async Task<ApiResponse<InventoryTransaction>> GetWithItems(Guid id)
    {
        try
        {
            var transaction = await _repository.GetWithItems(id);
            return new ApiResponse<InventoryTransaction>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = transaction
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<InventoryTransaction>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetByBranch(Guid branchId)
    {
        try
        {
            var transactions = await _repository.GetByBranch(branchId);
            return new ApiResponse<IEnumerable<InventoryTransaction>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = transactions
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<InventoryTransaction>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<InventoryTransaction>>> GetByTransactionType(InventoryTransactionType transactionType)
    {
        try
        {
            var transactions = await _repository.GetByTransactionType(transactionType);
            return new ApiResponse<IEnumerable<InventoryTransaction>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = transactions
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<InventoryTransaction>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime)
    {
        try
        {
            var periodResult = await _accountingIntegration.GetOpenPeriodForDateAsync(dateTime);
            if (!periodResult.IsSuccess || periodResult.Result is null)
            {
                return new ApiResponse<TransactionNumberDto>
                {
                    IsSuccess = false,
                    StatusCode = periodResult.StatusCode,
                    Errors = periodResult.Errors
                };
            }

            var financialPeriod = periodResult.Result;
            var result = new TransactionNumberDto
            {
                FinancialPeriodId = financialPeriod.Id,
                FinancialPeriodNumber = financialPeriod.YearNumber ?? string.Empty
            };

            var lastTransactionNumber = _repository.GetQuery()
                .Where(e => e.FinancialPeriodId == financialPeriod.Id)
                .ToList()
                .OrderByDescending(e => BigInteger.Parse(e.TransactionNumber))
                .Select(e => BigInteger.Parse(e.TransactionNumber))
                .FirstOrDefault();

            result.TransactionNumber = lastTransactionNumber == 0
                ? "1"
                : (lastTransactionNumber + 1).ToString();

            return new ApiResponse<TransactionNumberDto>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<TransactionNumberDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<MessageTemplate> { new MessageTemplate { MessageKey = $"Failed to generate transaction number: {ex.Message}" } }
            };
        }
    }

    public override Task<ApiResponse<InventoryTransaction>> Create(InventoryTransactionCreateCommand command, bool isValidate = true)
        => _transactionCoordinator.ExecuteAsync(ct => CreateWithinTransactionAsync(command, isValidate, ct));

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

        var periodResult = await _accountingIntegration.GetOpenPeriodForDateAsync(command.TransactionDate);
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

        var lastTransactionNumber = _repository.GetQuery()
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
                var stockBalanceResp = await _stockBalanceService.GetCurrentBalance(item.VariantId, item.PackingUnitId, command.BranchId);
                var currentBalance = stockBalanceResp.Result;
                if (currentBalance < item.Quantity)
                {
                    return new ApiResponse<InventoryTransaction>
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = [new MessageTemplate { MessageKey = "InsufficientStockBalance" }]
                    };
                }
            }
        }

        var createdTransaction = await _repository.Add(transaction);

        foreach (var item in command.Items)
        {
            var unitCost = item.TotalCost / item.Quantity;
            var isReceipt = command.TransactionType == InventoryTransactionType.Receipt;
            var updateResult = await _stockBalanceService.UpdateStockBalance(
                item.VariantId,
                item.PackingUnitId,
                command.BranchId,
                item.Quantity,
                unitCost,
                isReceipt);
            if (!updateResult.IsSuccess)
                throw new Exception("Failed To Add Stock Balance");
        }

        var totalAmount = command.Items.Sum(i => i.TotalCost);
        var glDirection = command.TransactionType == InventoryTransactionType.Receipt
            ? InventoryGlDirection.Receipt
            : InventoryGlDirection.Issue;

        var entryResult = await _accountingIntegration.PostInventoryTransactionEntryAsync(
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

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(InventoryTransactionCreateCommand command)
    {
        var validationResult = await base.ValidateCreate(command);

        var periodResult = await _accountingIntegration.GetOpenPeriodForDateAsync(command.TransactionDate);
        if (!periodResult.IsSuccess)
        {
            validationResult.isValid = false;
            validationResult.errors.Add("NotFoundCurrentFinancialPeriod");
        }

        return validationResult;
    }
}
