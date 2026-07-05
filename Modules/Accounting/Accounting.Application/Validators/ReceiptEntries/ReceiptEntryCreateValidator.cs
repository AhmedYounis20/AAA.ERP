using Accounting.Application.Commands.ReceiptEntries;
using Accounting.Application.Validators.Entries;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.ReceiptEntries;

public class ReceiptEntryCreateValidator : BaseCreateValidator<ReceiptEntryCreateCommand, Entry>
{
    public ReceiptEntryCreateValidator()
    {
        Include(new EntryHeaderRulesValidator());
        RuleFor(e => e.FinancialTransactions).NotEmpty().WithMessage(ValidationMessages.EntryFinancialTransactionsRequired);
        RuleForEach(e => e.CostCenters).SetValidator(new EntryCostCenterValidator()).When(e => e.CostCenters != null && e.CostCenters.Any());
    }
}
