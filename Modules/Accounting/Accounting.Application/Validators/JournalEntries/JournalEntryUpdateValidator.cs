using Accounting.Application.Commands.JournalEntries;
using Accounting.Application.Validators.Entries;
using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.JournalEntries;

public class JournalEntryUpdateValidator : BaseUpdateValidator<JournalEntryUpdateCommand, Entry>
{
    public JournalEntryUpdateValidator()
    {
        Include(new EntryHeaderRulesValidator());
        RuleFor(e => e.FinancialTransactions).NotEmpty().WithMessage(ValidationMessages.EntryFinancialTransactionsRequired);
    }
}
