using Accounting.Application.Commands.JournalEntries;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;

namespace Accounting.Application.Validators.JournalEntries;

public class JournalEntryUpdateValidator : BaseUpdateValidator<JournalEntryUpdateCommand, Entry>
{
    public JournalEntryUpdateValidator()
    {
        _ = RuleFor(e => e.BranchId).NotEmpty().WithMessage("BranchIdIsRequired");
        _ = RuleFor(e => e.CurrencyId).NotEmpty().WithMessage("CurrencyIdIsRequired");
        _ = RuleFor(e => e.FinancialPeriodId).NotEmpty().WithMessage("FinancialPeriodIdIsRequired");
        _ = RuleFor(e => e.Notes).MaximumLength(500).WithMessage("NotesMaximumLength");
        _ = RuleFor(e => e.ReceiverName).MaximumLength(100).WithMessage("ReceiverNameMaximumLength");
        _ = RuleFor(e => e.DocumentNumber).MaximumLength(100).WithMessage("DocumentNumberMaximumLength");
        _ = RuleFor(e => e.FinancialTransactions).NotEmpty().WithMessage("EntryFinancialTransactionsRequired");
    }
}
