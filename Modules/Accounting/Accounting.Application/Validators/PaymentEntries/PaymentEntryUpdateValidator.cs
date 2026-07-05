using Accounting.Application.Commands.PaymentEntries;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Application.Validators.Entries;
using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.PaymentEntries;

public class PaymentEntryUpdateValidator : BaseUpdateValidator<PaymentEntryUpdateCommand, Entry>
{
    public PaymentEntryUpdateValidator()
    {
        _ = RuleFor(e => e.BranchId).NotEmpty().WithMessage(ValidationMessages.BranchIdIsRequired);
        _ = RuleFor(e => e.CurrencyId).NotEmpty().WithMessage(ValidationMessages.CurrencyIdIsRequired);
        _ = RuleFor(e => e.FinancialPeriodId).NotEmpty().WithMessage(ValidationMessages.FinancialPeriodIdIsRequired);
        _ = RuleFor(e => e.Notes).MaximumLength(500).WithMessage(ValidationMessages.NotesMaximumLength);
        _ = RuleFor(e => e.ReceiverName).MaximumLength(100).WithMessage(ValidationMessages.ReceiverNameMaximumLength);
        _ = RuleFor(e => e.DocumentNumber).MaximumLength(100).WithMessage(ValidationMessages.DocumentNumberMaximumLength);
        _ = RuleFor(e => e.FinancialTransactions).NotEmpty().WithMessage(ValidationMessages.EntryFinancialTransactionsRequired);
        _ = RuleForEach(e => e.CostCenters).SetValidator(new EntryCostCenterValidator()).When(e => e.CostCenters != null && e.CostCenters.Any());
    }
}
