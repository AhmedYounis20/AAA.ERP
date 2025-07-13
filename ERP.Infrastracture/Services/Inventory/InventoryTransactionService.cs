using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.InventoryTransactions;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace ERP.Infrastracture.Services.Inventory;

public class InventoryTransactionService : BaseService<InventoryTransaction, InventoryTransactionCreateCommand, InventoryTransactionUpdateCommand>, IInventoryTransactionService
{
    private readonly IInventoryTransactionRepository _repository;
    private readonly IStockBalanceService _stockBalanceService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _dbContext;

    public InventoryTransactionService(
        IInventoryTransactionRepository repository,
        IStockBalanceService stockBalanceService,
        IUnitOfWork unitOfWork,
        ApplicationDbContext dbContext) : base(repository)
    {
        _repository = repository;
        _stockBalanceService = stockBalanceService;
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
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
                ErrorMessages = [ex.Message]
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
                ErrorMessages = [ex.Message]
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
                ErrorMessages = [ex.Message]
            };
        }
    }

    public async Task<ApiResponse<TransactionNumberDto>> GetTransactionNumber(DateTime dateTime)
    {
        try
        {
            var result = new TransactionNumberDto();

            // Get financial period for the given date (same logic as entry services)
            var financialPeriod = await _dbContext.Set<FinancialPeriod>()
                .FirstOrDefaultAsync(e => e.StartDate <= dateTime && e.EndDate > dateTime);
            if (financialPeriod == null)
            {
                return new ApiResponse<TransactionNumberDto>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.Found,
                    ErrorMessages = new List<string> { "NotFoundCurrentFinancialPeriod" }
                };
            }

            result.FinancialPeriodId = financialPeriod.Id;
            result.FinancialPeriodNumber = financialPeriod.YearNumber ?? string.Empty;

            // Generate transaction number following the same pattern as Entry number generation
            var lastTransactionNumber = _repository.GetQuery()
                .Where(e => e.FinancialPeriodId == financialPeriod.Id)
                .ToList()
                .OrderByDescending(e => BigInteger.Parse(e.TransactionNumber))
                .Select(e => BigInteger.Parse(e.TransactionNumber))
                .FirstOrDefault();

            string transactionNumber;
            if (lastTransactionNumber == 0)
            {
                // First transaction for this financial period
                transactionNumber = "1";
            }
            else
            {
                // Increment the last transaction number
                transactionNumber = (lastTransactionNumber + 1).ToString();
            }

            result.TransactionNumber = transactionNumber;

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
                ErrorMessages = [$"Failed to generate transaction number: {ex.Message}"]
            };
        }
    }

    public override async Task<ApiResponse<InventoryTransaction>> Create(InventoryTransactionCreateCommand command, bool isValidate = true)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            // Validate first
            if (isValidate)
            {
                var validationResult = await ValidateCreate(command);
                if (!validationResult.isValid)
                {
                    return new ApiResponse<InventoryTransaction>
                    {
                        IsSuccess = false,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ErrorMessages = validationResult.errors
                    };
                }
            }

            // Get financial period for the transaction date
            var financialPeriod = await _dbContext.Set<FinancialPeriod>()
                .FirstOrDefaultAsync(e => e.StartDate <= command.TransactionDate && e.EndDate > command.TransactionDate);
            if (financialPeriod == null)
            {
                return new ApiResponse<InventoryTransaction>
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ErrorMessages = ["NotFoundCurrentFinancialPeriod"]
                };
            }

            // Generate transaction number following the same pattern as Entry number generation
            var lastTransactionNumber = _repository.GetQuery()
                .Where(e => e.FinancialPeriodId == financialPeriod.Id)
                .ToList()
                .OrderByDescending(e => BigInteger.Parse(e.TransactionNumber))
                .Select(e => BigInteger.Parse(e.TransactionNumber))
                .FirstOrDefault();

            string transactionNumber;
            if (lastTransactionNumber == 0)
            {
                // First transaction for this financial period
                transactionNumber = "1";
            }
            else
            {
                // Increment the last transaction number
                transactionNumber = (lastTransactionNumber + 1).ToString();
            }

            // Create the transaction
            var transaction = command.Adapt<InventoryTransaction>();
            transaction.TransactionNumber = transactionNumber;
            transaction.FinancialPeriodId = financialPeriod.Id;
            transaction.Items = command.Items.Select(item => new InventoryTransactionItem
            {
                ItemId = item.ItemId,
                PackingUnitId = item.PackingUnitId,
                Quantity = item.Quantity,
                TotalCost = item.TotalCost
            }).ToList();

            // --- Handle Export (Issue) validation ---
            if (command.TransactionType == InventoryTransactionType.Issue)
            {
                var insufficientItems = new List<string>();
                foreach (var item in command.Items)
                {
                    // Get current balance using default packing unit
                    var stockBalanceResp = await _stockBalanceService.GetCurrentBalance(item.ItemId, item.PackingUnitId, command.BranchId);
                    var currentBalance = stockBalanceResp.Result;
                    if (currentBalance < item.Quantity)
                    {
                        insufficientItems.Add($"ItemId: {item.ItemId}, PackingUnitId: {item.PackingUnitId}, Requested: {item.Quantity}, Available: {currentBalance}");
                    }
                }
                if (insufficientItems.Any())
                {
                    await _unitOfWork.RollbackAsync();
                    return new ApiResponse<InventoryTransaction>
                    {
                        IsSuccess = false,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ErrorMessages = ["InsufficientStockBalance"]
                    };
                }
            }

            var createdTransaction = await _repository.Add(transaction);

            // Update stock balances
            foreach (var item in command.Items)
            {
                var unitCost = item.TotalCost / item.Quantity;
                bool isReceipt = command.TransactionType == InventoryTransactionType.Receipt;
                var updateResult = await _stockBalanceService.UpdateStockBalance(
                    item.ItemId,
                    item.PackingUnitId,
                    command.BranchId,
                    item.Quantity,
                    unitCost,
                    isReceipt);
                if (!updateResult.IsSuccess)
                    throw new Exception("Failed To Add Stock Balance");
            }

            await _unitOfWork.CommitAsync();

            return new ApiResponse<InventoryTransaction>
            {
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.Created,
                Result = createdTransaction
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return new ApiResponse<InventoryTransaction>
            {
                IsSuccess = false,
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                ErrorMessages = [ex.Message]
            };
        }
    }

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(InventoryTransactionCreateCommand command)
    {
        var validationResult = await base.ValidateCreate(command);

        // Validate financial period exists and matches the transaction date
        var financialPeriod = await _dbContext.Set<FinancialPeriod>()
            .FirstOrDefaultAsync(e => e.StartDate <= command.TransactionDate && e.EndDate > command.TransactionDate);
        if (financialPeriod == null)
        {
            validationResult.isValid = false;
            validationResult.errors.Add("NotFoundCurrentFinancialPeriod");
        }

        return validationResult;
    }
} 