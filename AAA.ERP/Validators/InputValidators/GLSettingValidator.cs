using AAA.ERP.InputModels;
using AAA.ERP.Models.Entities.GLSettings;
using AAA.ERP.Validators.InputValidators.BaseValidators;
using FluentValidation;

namespace AAA.ERP.Validators.InputValidators;

public class GLSettingValidator : BaseInputValidator<GLSettingInputModel>
{
    public GLSettingValidator() {

        _ = RuleFor(e => e.DecimalDigitsNumber).GreaterThanOrEqualTo((byte)0).WithMessage("DecimalDigitsMINValue").LessThanOrEqualTo((byte)0).WithMessage("DecimalDigitsMAXValue");
        _ = RuleFor(e => e.MonthDays)
            .GreaterThanOrEqualTo((byte)1).When(e=>e.DepreciationApplication.Equals(DepreciationApplication.Monthly)).WithMessage("MonthDaysMINValue")
            .LessThanOrEqualTo((byte)31).When(e => e.DepreciationApplication.Equals(DepreciationApplication.Monthly)).WithMessage("MonthDaysMAXValue")
            .Equal((byte)0).When(e => e.DepreciationApplication.Equals(DepreciationApplication.WithYearClosed)).WithMessage("MonthDaysYearClosedDays");
    }
}