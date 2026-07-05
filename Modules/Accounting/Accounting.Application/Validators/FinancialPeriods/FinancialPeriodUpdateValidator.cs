using Accounting.Application.Commands.FinancialPeriods;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using FluentValidation;

namespace Accounting.Application.Validators.FinancialPeriods;

public class FinancialPeriodUpdateValidator : BaseUpdateValidator<FinancialPeriodUpdateCommand, FinancialPeriod>
{
    public FinancialPeriodUpdateValidator() : base()
    {
        _ = RuleFor(e => e.YearNumber).NotEmpty().WithMessage("FinancialPeriodRequiredYearNumber").MaximumLength(50).WithMessage("FinancialPeriodMaximumLength");
    }
}
