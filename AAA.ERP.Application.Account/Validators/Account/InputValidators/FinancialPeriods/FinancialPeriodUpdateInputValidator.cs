using ERP.Application.Validators.Account.InputValidators.BaseValidators;
using ERP.Domain.Commands.Account.FinancialPeriods;
using FluentValidation;

namespace ERP.Application.Validators.Account.InputValidators.FinancialPeriods;

public class FinancialPeriodUpdateValidator : BaseInputValidator<FinancialPeriodUpdateInputModel>
{
    public FinancialPeriodUpdateValidator()
    {
        _ = RuleFor(e => e.YearNumber).NotEmpty().WithMessage("FinancialPeriodRequiredYearNumber").MaximumLength(50).WithMessage("FinancialPeriodMaximumLength");
    }
}