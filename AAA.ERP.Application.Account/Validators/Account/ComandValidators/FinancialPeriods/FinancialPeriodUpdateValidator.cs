using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using ERP.Domain.Commands.Account.FinancialPeriods;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;
using FluentValidation;

namespace ERP.Application.Validators.Account.ComandValidators.FinancialPeriods;

public class FinancialPeriodUpdateValidator : BaseUpdateValidator<FinancialPeriodUpdateCommand, FinancialPeriod>
{
    public FinancialPeriodUpdateValidator() : base()
    {
        _ = RuleFor(e => e.YearNumber).NotEmpty().WithMessage("FinancialPeriodRequiredYearNumber").MaximumLength(50).WithMessage("FinancialPeriodMaximumLength");
    }
}