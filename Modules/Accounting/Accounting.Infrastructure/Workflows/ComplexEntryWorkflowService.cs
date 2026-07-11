using Accounting.Application.Commands.Entries;
using Accounting.Application.Dtos.Entries;
using Accounting.Application.Repositories.Account;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.Entries;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Infrastructure.Workflows;

/// <summary>
/// Complex entry write orchestration (debit/credit pairing) delegating core persistence to entry workflow.
/// </summary>
public class ComplexEntryWorkflowService(
    IApplicationDbContext dbContext,
    IEntryWorkflowService entryWorkflow) : IComplexEntryWorkflowService
{
    public async Task<ApiResponse<Entry>> Create(ComplexEntryCreateCommand command)
    {
        try
        {
            var validationResult = await ValidateCreate(command);
            if (!validationResult.isValid)
            {
                return new ApiResponse<Entry>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            var financialTransactions = new List<FinancialTransaction>();
            var i = 1;
            command.FinancialTransactions.ForEach(e => e.OrderNumber = i++);
            MapFinancialTransactions(command.FinancialTransactions, financialTransactions);

            var createEntryCommand = command.Adapt<EntryCreateCommand>();
            createEntryCommand.FinancialTransactions = financialTransactions;

            return await entryWorkflow.Create(createEntryCommand);
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return new ApiResponse<Entry>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<Entry>> Update(ComplexEntryUpdateCommand command)
    {
        try
        {
            var financialTransactions = new List<FinancialTransaction>();
            var i = 1;
            command.FinancialTransactions.ForEach(e => e.OrderNumber = i++);
            MapFinancialTransactions(command.FinancialTransactions, financialTransactions);

            var updateEntryCommand = command.Adapt<EntryUpdateCommand>();
            updateEntryCommand.FinancialTransactions = financialTransactions;

            return await entryWorkflow.Update(updateEntryCommand);
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return new ApiResponse<Entry>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    private static void MapFinancialTransactions(
        IEnumerable<ComplexFinancialTransactionDto> complexTransactions,
        List<FinancialTransaction> financialTransactions)
    {
        foreach (var transaction in complexTransactions)
        {
            transaction.Id = Guid.NewGuid();
            transaction.ComplementId = Guid.NewGuid();

            var debitTransaction = transaction.Adapt<FinancialTransaction>();
            var creditTransaction = transaction.Adapt<FinancialTransaction>();

            debitTransaction.ChartOfAccountId = transaction.DebitAccountId;
            debitTransaction.Id = transaction.Id.Equals(Guid.Empty) ? Guid.NewGuid() : transaction.Id;
            creditTransaction.Id = transaction.ComplementId.Equals(Guid.Empty) ? Guid.NewGuid() : transaction.ComplementId;
            creditTransaction.ChartOfAccountId = transaction.CreditAccountId;
            creditTransaction.AccountNature = debitTransaction.AccountNature == AccountNature.Debit
                ? AccountNature.Credit
                : AccountNature.Debit;
            creditTransaction.ComplementTransactionId = debitTransaction.Id;
            debitTransaction.ComplementTransactionId = creditTransaction.Id;

            financialTransactions.Add(debitTransaction);
            financialTransactions.Add(creditTransaction);
        }
    }

    private async Task<(bool isValid, List<string> errors)> ValidateCreate(ComplexEntryCreateCommand command)
    {
        var validationResult = (isValid: true, errors: new List<string>());

        var financialPeriod = await dbContext.Set<FinancialPeriod>()
            .FirstOrDefaultAsync(e => e.StartDate <= command.EntryDate && e.EndDate > command.EntryDate);
        if (financialPeriod == null || command.FinancialPeriodId != financialPeriod.Id)
        {
            validationResult.isValid = false;
            validationResult.errors.Add(ErrorMessages.FinancialPeriodNotFound);
        }

        var entryWithSameNumber = await dbContext.Set<Entry>()
            .Where(e => e.EntryNumber == command.EntryNumber && e.FinancialPeriodId == command.FinancialPeriodId)
            .FirstOrDefaultAsync();
        if (entryWithSameNumber != null)
        {
            validationResult.isValid = false;
            validationResult.errors.Add(ErrorMessages.EntryNumberExists);
        }

        return validationResult;
    }
}
