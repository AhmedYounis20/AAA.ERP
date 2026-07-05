using Accounting.Application.Commands.JournalEntries;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;
using Shared.Constants;

namespace Accounting.Application.Validators.JournalEntries;

public class JournalEntryUpdateValidator : BaseUpdateValidator<JournalEntryUpdateCommand, Entry>
{
    public JournalEntryUpdateValidator()
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
