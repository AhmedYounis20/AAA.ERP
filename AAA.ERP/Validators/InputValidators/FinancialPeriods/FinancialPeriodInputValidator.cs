using AAA.ERP.InputModels;
using AAA.ERP.InputModels.FinancialPeriods;
using AAA.ERP.Models.Entities.FinancialPeriods;
using AAA.ERP.Models.Entities.GLSettings;
using AAA.ERP.Validators.InputValidators.BaseValidators;
using FluentValidation;

namespace AAA.ERP.Validators.InputValidators.FinancialPeriods;

public class FinancialPeriodInputValidator : BaseInputValidator<FinancialPeriodInputModel>
{
    public FinancialPeriodInputValidator()
    {
        _ = RuleFor(e => e.YearNumber).NotEmpty().WithMessage("FinancialPeriodRequiredYearNumber").MaximumLength(50).WithMessage("FinancialPeriodMaximumLength");
        _ = RuleFor(e => e.StartDate).NotEmpty().WithMessage("FinancialPeriodStartDateRequired");
        _ = RuleFor(e => e.PeriodTypeByMonth).Must(IsValidPeriodType).WithMessage("NotValidPeriodType");
    }

    public bool IsValidPeriodType(byte periodType)
    {
        return periodType == FinancialPeriodType.OneMonth
               || periodType == FinancialPeriodType.ThreeMonths
               || periodType == FinancialPeriodType.SixMonths
               || periodType == FinancialPeriodType.OneYear;
    }
}