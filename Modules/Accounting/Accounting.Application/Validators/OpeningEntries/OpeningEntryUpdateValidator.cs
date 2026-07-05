using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Application.Commands.OpeningEntries;
using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;
using Shared.Constants;

namespace Accounting.Application.Validators.OpeningEntries;

public class OpeningEntryUpdateValidator : BaseUpdateValidator<OpeningEntryUpdateCommand, Entry>
{
    public OpeningEntryUpdateValidator() : base()
    {
        _ = RuleFor(e => e.BranchId).NotEmpty().WithMessage(ErrorMessages.BranchIdIsRequired);
        _ = RuleFor(e => e.CurrencyId).NotEmpty().WithMessage(ErrorMessages.CurrencyIdIsRequired);
        _ = RuleFor(e => e.FinancialPeriodId).NotEmpty().WithMessage(ErrorMessages.FinancialPeriodIdIsRequired);
        _ = RuleFor(e => e.Notes).MaximumLength(500).WithMessage(ErrorMessages.NotesMaximumLength);
        _ = RuleFor(e => e.ReceiverName).MaximumLength(100).WithMessage(ErrorMessages.ReceiverNameMaximumLength);
        _ = RuleFor(e => e.DocumentNumber).MaximumLength(100).WithMessage(ErrorMessages.DocumentNumberMaximumLength);
        _ = RuleFor(e => e.FinancialTransactions).NotEmpty().WithMessage(ErrorMessages.EntryFinancialTransactionsRequired);
    }
}