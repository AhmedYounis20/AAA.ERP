using AAA.ERP.Validators.InputValidators.BaseValidators;
using Domain.Account.InputModels.FinancialPeriods;
using FluentValidation;

namespace Domain.Account.Validators.InputValidators.FinancialPeriods;

public class FinancialPeriodUpdateValidator : BaseInputValidator<FinancialPeriodUpdateInputModel>
{
    public FinancialPeriodUpdateValidator()
    {
        _ = RuleFor(e => e.YearNumber).NotEmpty().WithMessage("FinancialPeriodRequiredYearNumber").MaximumLength(50).WithMessage("FinancialPeriodMaximumLength");
    }
}