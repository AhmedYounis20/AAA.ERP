using Domain.Account.Commands.Entries.OpeningEntries;
using Domain.Account.Models.Entities.Entries;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using FluentValidation;

namespace ERP.Application.Validators.Account.ComandValidators.Entries.OpeningEntries;

public class OpeningEntryCreateValidator : BaseCreateValidator<OpeningEntryCreateCommand, Entry>
{
    public OpeningEntryCreateValidator() : base()
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