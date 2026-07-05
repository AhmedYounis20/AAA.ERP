using Accounting.Application.Commands.ReceiptEntries;
using Accounting.Application.Validators.Entries;
using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.ReceiptEntries;

public class ReceiptEntryUpdateValidator : BaseUpdateValidator<ReceiptEntryUpdateCommand, Entry>
{
    public ReceiptEntryUpdateValidator()
    {
        Include(new EntryHeaderRulesValidator());
        RuleFor(e => e.FinancialTransactions).NotEmpty().WithMessage(ValidationMessages.EntryFinancialTransactionsRequired);
        RuleForEach(e => e.CostCenters).SetValidator(new EntryCostCenterValidator()).When(e => e.CostCenters != null && e.CostCenters.Any());
    }
}
