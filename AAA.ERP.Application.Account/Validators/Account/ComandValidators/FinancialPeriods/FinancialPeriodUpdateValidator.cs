using Domain.Account.Commands.FinancialPeriods;
using Domain.Account.Models.Entities.FinancialPeriods;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using FluentValidation;

namespace ERP.Application.Validators.Account.ComandValidators.FinancialPeriods;

public class FinancialPeriodUpdateValidator : BaseUpdateValidator<FinancialPeriodUpdateCommand, FinancialPeriod>
{
    public FinancialPeriodUpdateValidator() : base()
    {
        _ = RuleFor(e => e.YearNumber).NotEmpty().WithMessage("FinancialPeriodRequiredYearNumber").MaximumLength(50).WithMessage("FinancialPeriodMaximumLength");
    }
}