using Account.Domain.Commands.FinancialPeriods;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;
using Account.Domain.Models.Entities.FinancialPeriods;
using FluentValidation;

namespace Account.Application.Validators.CommandValidators.FinancialPeriods;

public class FinancialPeriodUpdateValidator : BaseUpdateValidator<FinancialPeriodUpdateCommand, FinancialPeriod>
{
    public FinancialPeriodUpdateValidator() : base()
    {
        _ = RuleFor(e => e.YearNumber).NotEmpty().WithMessage("FinancialPeriodRequiredYearNumber").MaximumLength(50).WithMessage("FinancialPeriodMaximumLength");
    }
}