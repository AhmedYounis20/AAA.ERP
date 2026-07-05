using Accounting.Application.Commands.OpeningEntries;
using Accounting.Application.Validators.Entries;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.OpeningEntries;

public class OpeningEntryCreateValidator : BaseCreateValidator<OpeningEntryCreateCommand, Entry>
{
    public OpeningEntryCreateValidator() : base()
    {
        Include(new EntryHeaderRulesValidator());
        RuleFor(e => e.FinancialTransactions).NotEmpty().WithMessage(ValidationMessages.EntryFinancialTransactionsRequired);
    }
}
