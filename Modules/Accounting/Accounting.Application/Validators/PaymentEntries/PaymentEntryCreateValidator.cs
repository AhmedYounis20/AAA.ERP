using Accounting.Application.Commands.PaymentEntries;
using Accounting.Application.Validators.Entries;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.PaymentEntries;

public class PaymentEntryCreateValidator : BaseCreateValidator<PaymentEntryCreateCommand, Entry>
{
    public PaymentEntryCreateValidator()
    {
        Include(new EntryHeaderRulesValidator());
        RuleFor(e => e.FinancialTransactions).NotEmpty().WithMessage(ValidationMessages.EntryFinancialTransactionsRequired);
        RuleForEach(e => e.CostCenters).SetValidator(new EntryCostCenterValidator()).When(e => e.CostCenters != null && e.CostCenters.Any());
    }
}
