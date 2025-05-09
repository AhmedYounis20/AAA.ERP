using Domain.Account.Commands.Entries.ReceiptEntries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Validators.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using FluentValidation;

namespace Domain.Account.Validators.ComandValidators.Entries.ReceiptEntries;

public class ReceiptEntryCreateValidator : BaseCreateValidator<ReceiptEntryCreateCommand, Entry>
{
    public ReceiptEntryCreateValidator()
    {
        _ = RuleFor(e => e.BranchId).NotEmpty().WithMessage("BranchIdIsRequired");
        _ = RuleFor(e => e.CurrencyId).NotEmpty().WithMessage("CurrencyIdIsRequired");
        _ = RuleFor(e => e.FinancialPeriodId).NotEmpty().WithMessage("FinancialPeriodIdIsRequired");
        _ = RuleFor(e => e.Notes).MaximumLength(500).WithMessage("NotesMaximumLength");
        _ = RuleFor(e => e.ReceiverName).MaximumLength(100).WithMessage("ReceiverNameMaximumLength");
        _ = RuleFor(e => e.DocumentNumber).MaximumLength(100).WithMessage("DocumentNumberMaximumLength");
        _ = RuleFor(e => e.FinancialTransactions).NotEmpty().WithMessage("EntryFinancialTransactionsRequired");
        _ = RuleForEach(e=>e.CostCenters).SetValidator(new EntryCostCenterValidator()).When(e=> e.CostCenters != null && e.CostCenters.Any());
    }
}