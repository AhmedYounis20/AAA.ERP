using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Application.Commands.OpeningEntries;
using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.OpeningEntries;

public class OpeningEntryUpdateValidator : BaseUpdateValidator<OpeningEntryUpdateCommand, Entry>
{
    public OpeningEntryUpdateValidator() : base()
    {
        _ = RuleFor(e => e.BranchId).NotEmpty().WithMessage(ValidationMessages.BranchIdIsRequired);
        _ = RuleFor(e => e.CurrencyId).NotEmpty().WithMessage(ValidationMessages.CurrencyIdIsRequired);
        _ = RuleFor(e => e.FinancialPeriodId).NotEmpty().WithMessage(ValidationMessages.FinancialPeriodIdIsRequired);
        _ = RuleFor(e => e.Notes).MaximumLength(500).WithMessage(ValidationMessages.NotesMaximumLength);
        _ = RuleFor(e => e.ReceiverName).MaximumLength(100).WithMessage(ValidationMessages.ReceiverNameMaximumLength);
        _ = RuleFor(e => e.DocumentNumber).MaximumLength(100).WithMessage(ValidationMessages.DocumentNumberMaximumLength);
        _ = RuleFor(e => e.FinancialTransactions).NotEmpty().WithMessage(ValidationMessages.EntryFinancialTransactionsRequired);
    }
}