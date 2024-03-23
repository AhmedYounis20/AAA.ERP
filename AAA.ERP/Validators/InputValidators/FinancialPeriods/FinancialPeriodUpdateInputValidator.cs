using AAA.ERP.InputModels.FinancialPeriods;
using AAA.ERP.Models.Entities.FinancialPeriods;
using AAA.ERP.Validators.InputValidators.BaseValidators;
using FluentValidation;

namespace AAA.ERP.Validators.InputValidators.FinancialPeriods;

public class FinancialPeriodUpdateValidator : BaseInputValidator<FinancialPeriodUpdateInputModel>
{
    public FinancialPeriodUpdateValidator()
    {
        _ = RuleFor(e => e.YearNumber).NotEmpty().WithMessage("FinancialPeriodRequiredYearNumber").MaximumLength(50).WithMessage("FinancialPeriodMaximumLength");
    }
}