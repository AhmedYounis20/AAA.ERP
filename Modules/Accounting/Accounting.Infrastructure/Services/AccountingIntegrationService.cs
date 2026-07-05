using Accounting.Contracts.Integration;
using Domain.Account.Models.Dtos.Entry;
using ERP.Application.Data;
using Accounting.Application.Commands.CompinedEntries;
using ERP.Domain.Models.Entities.Account.ChartOfAccounts;
using ERP.Domain.Models.Entities.Account.Currencies;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;
using ERP.Domain.Models.Entities.Account.SubLeadgers;
using MediatR;
using Shared.Responses;

namespace Accounting.Infrastructure.Services;

public class AccountingIntegrationService : IAccountingIntegrationService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMediator _mediator;

    public AccountingIntegrationService(IApplicationDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<ApiResponse<FinancialPeriodInfo>> GetOpenPeriodForDateAsync(
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        var financialPeriod = await _dbContext.Set<FinancialPeriod>()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                e => e.StartDate <= date && e.EndDate > date,
                cancellationToken);

        if (financialPeriod is null)
        {
            return new ApiResponse<FinancialPeriodInfo>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = "NotFoundCurrentFinancialPeriod" }]
            };
        }

        return new ApiResponse<FinancialPeriodInfo>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = new FinancialPeriodInfo(
                financialPeriod.Id,
                financialPeriod.StartDate,
                financialPeriod.EndDate,
                financialPeriod.YearNumber)
        };
    }

    public async Task<ApiResponse<BranchAccountInfo>> GetBranchAccountAsync(
        Guid branchId,
        CancellationToken cancellationToken = default)
    {
        var branch = await _dbContext.Set<Branch>()
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == branchId, cancellationToken);

        if (branch is null)
        {
            return new ApiResponse<BranchAccountInfo>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = "BranchOrBranchAccountNotFound" }]
            };
        }

        return new ApiResponse<BranchAccountInfo>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = new BranchAccountInfo(branch.Id, branch.ChartOfAccountId)
        };
    }

    public async Task<ApiResponse<Guid>> ValidatePartyAccountAsync(
        Guid chartOfAccountId,
        CancellationToken cancellationToken = default)
    {
        var partyAccount = await _dbContext.Set<ChartOfAccount>()
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == chartOfAccountId, cancellationToken);

        if (partyAccount is null)
        {
            return new ApiResponse<Guid>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = "PartyAccountNotFound" }]
            };
        }

        return new ApiResponse<Guid>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = partyAccount.Id
        };
    }

    public async Task<ApiResponse<Guid>> PostInventoryTransactionEntryAsync(
        InventoryTransactionPostingRequest request,
        CancellationToken cancellationToken = default)
    {
        var defaultCurrency = await _dbContext.Set<Currency>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.IsDefault, cancellationToken);

        if (defaultCurrency is null)
        {
            return new ApiResponse<Guid>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = "DefaultCurrencyNotFound" }]
            };
        }

        var branchAccountResult = await GetBranchAccountAsync(request.BranchId, cancellationToken);
        if (!branchAccountResult.IsSuccess || branchAccountResult.Result?.ChartOfAccountId is null)
        {
            return new ApiResponse<Guid>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = branchAccountResult.Errors ??
                         [new MessageTemplate { MessageKey = "BranchOrBranchAccountNotFound" }]
            };
        }

        var partyAccountResult = await ValidatePartyAccountAsync(request.TransactionPartyId, cancellationToken);
        if (!partyAccountResult.IsSuccess)
        {
            return new ApiResponse<Guid>
            {
                IsSuccess = false,
                StatusCode = partyAccountResult.StatusCode,
                Errors = partyAccountResult.Errors
            };
        }

        var branchAccountId = branchAccountResult.Result.ChartOfAccountId!.Value;
        var partyAccountId = partyAccountResult.Result;
        var isReceipt = request.Direction == InventoryGlDirection.Receipt;

        var command = new CompinedEntryCreateCommand
        {
            BranchId = request.BranchId,
            CurrencyId = defaultCurrency.Id,
            ExchangeRate = defaultCurrency.ExchangeRate,
            EntryDate = request.TransactionDate,
            FinancialPeriodId = request.FinancialPeriodId,
            FinancialTransactions =
            [
                new ComplexFinancialTransactionDto
                {
                    DebitAccountId = isReceipt ? branchAccountId : partyAccountId,
                    CreditAccountId = isReceipt ? partyAccountId : branchAccountId,
                    Amount = request.TotalAmount,
                    PaymentType = PaymentType.None
                }
            ]
        };

        var entryResult = await _mediator.Send(command, cancellationToken);
        if (!entryResult.IsSuccess || entryResult.Result is null)
        {
            return new ApiResponse<Guid>
            {
                IsSuccess = false,
                StatusCode = entryResult.StatusCode,
                Errors = entryResult.Errors
            };
        }

        return new ApiResponse<Guid>
        {
            IsSuccess = true,
            StatusCode = entryResult.StatusCode,
            Result = entryResult.Result.Id
        };
    }

    public async Task<ApiResponse<Guid>> PostInventoryTransferEntryAsync(
        InventoryTransferPostingRequest request,
        CancellationToken cancellationToken = default)
    {
        var defaultCurrency = await _dbContext.Set<Currency>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.IsDefault, cancellationToken);

        if (defaultCurrency is null)
        {
            return new ApiResponse<Guid>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = "DefaultCurrencyNotFound" }]
            };
        }

        var command = new CompinedEntryCreateCommand
        {
            BranchId = request.BranchId,
            CurrencyId = defaultCurrency.Id,
            ExchangeRate = defaultCurrency.ExchangeRate,
            EntryDate = request.EntryDate,
            FinancialPeriodId = request.FinancialPeriodId,
            FinancialTransactions =
            [
                new ComplexFinancialTransactionDto
                {
                    DebitAccountId = request.DebitAccountId,
                    CreditAccountId = request.CreditAccountId,
                    Amount = request.TotalAmount,
                    PaymentType = PaymentType.None
                }
            ]
        };

        var entryResult = await _mediator.Send(command, cancellationToken);
        if (!entryResult.IsSuccess || entryResult.Result is null)
        {
            return new ApiResponse<Guid>
            {
                IsSuccess = false,
                StatusCode = entryResult.StatusCode,
                Errors = entryResult.Errors
            };
        }

        return new ApiResponse<Guid>
        {
            IsSuccess = true,
            StatusCode = entryResult.StatusCode,
            Result = entryResult.Result.Id
        };
    }
}
