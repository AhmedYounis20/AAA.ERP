using Accounting.Application.Commands.OpeningEntries;
using Accounting.Application.Validators.Entries;
using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.OpeningEntries;

public class OpeningEntryUpdateValidator : BaseUpdateValidator<OpeningEntryUpdateCommand, Entry>
{
    public OpeningEntryUpdateValidator() : base()
    {
        Include(new EntryHeaderRulesValidator());
        RuleFor(e => e.FinancialTransactions).NotEmpty().WithMessage(ValidationMessages.EntryFinancialTransactionsRequired);
    }
}
