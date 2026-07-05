using Accounting.Application.Validators.Entries;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Application.Commands.CombinedEntries;
using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.CombinedEntries;

public class CombinedEntryUpdateValidator : BaseUpdateValidator<CombinedEntryUpdateCommand, Entry>
{
    public CombinedEntryUpdateValidator()
    {
        Include(new EntryHeaderRulesValidator());
        RuleFor(e => e.FinancialTransactions).NotEmpty().WithMessage(ValidationMessages.EntryFinancialTransactionsRequired);
        RuleForEach(e => e.CostCenters).SetValidator(new EntryCostCenterValidator()).When(e => e.CostCenters != null && e.CostCenters.Any());
    }
}
