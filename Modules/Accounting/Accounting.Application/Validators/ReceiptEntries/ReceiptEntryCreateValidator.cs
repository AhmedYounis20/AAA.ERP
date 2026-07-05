using Accounting.Application.Commands.ReceiptEntries;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Application.Validators.Entries;
using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;

namespace Accounting.Application.Validators.ReceiptEntries;

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
        _ = RuleForEach(e => e.CostCenters).SetValidator(new EntryCostCenterValidator()).When(e => e.CostCenters != null && e.CostCenters.Any());
    }
}
