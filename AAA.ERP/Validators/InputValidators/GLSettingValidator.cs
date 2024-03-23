using AAA.ERP.InputModels;
using AAA.ERP.Models.Entities.GLSettings;
using AAA.ERP.Validators.InputValidators.BaseValidators;
using FluentValidation;

namespace AAA.ERP.Validators.InputValidators;

public class GLSettingValidator : BaseInputValidator<GLSettingInputModel>
{
    public GLSettingValidator()
    {
        _ = RuleFor(e => e.DecimalDigitsNumber).GreaterThanOrEqualTo((byte)0).WithMessage("DecimalDigitsMINValue").LessThanOrEqualTo((byte)10).WithMessage("DecimalDigitsMAXValue");
        _ = RuleFor(e => e.MonthDays).InclusiveBetween((byte)1, (byte)31).When(e => e.DepreciationApplication.Equals(DepreciationApplication.Monthly)).WithMessage("MonthDaysMINValue");
        _ = RuleFor(e => e.MonthDays).Equal((byte)0).When(e => e.DepreciationApplication.Equals(DepreciationApplication.WithYearClosed)).WithMessage("MonthDaysMINValue");
    }
}