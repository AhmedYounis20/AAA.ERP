using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using ERP.Domain.Commands.Account.Entries.PaymentEntries;
using ERP.Domain.Models.Entities.Account.Entries;
using FluentValidation;

namespace ERP.Application.Validators.Account.ComandValidators.Entries.PaymentEntries;

public class PaymentEntryCreateValidator : BaseCreateValidator<PaymentEntryCreateCommand, Entry>
{
    public PaymentEntryCreateValidator()
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