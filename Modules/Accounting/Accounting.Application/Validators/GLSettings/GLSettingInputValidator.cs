using ERP.Application.Validators.Account.InputValidators.BaseValidators;
using Accounting.Domain.Commands;
using Accounting.Domain.Models.Entities.GLSettings;
using FluentValidation;

namespace Accounting.Application.Validators.GLSettings;

public class GLSettingInputValidator : BaseInputValidator<GLSettingInputModel>
{
    public GLSettingInputValidator() : base()
    {
        _ = RuleFor(e => e.DecimalDigitsNumber).GreaterThanOrEqualTo((byte)0).WithMessage("DecimalDigitsMINValue").LessThanOrEqualTo((byte)10).WithMessage("DecimalDigitsMAXValue");
        _ = RuleFor(e => e.MonthDays).InclusiveBetween((byte)1, (byte)31).When(e => e.DepreciationApplication.Equals(DepreciationApplication.Monthly)).WithMessage("MonthDaysMINValue");
    }
}
