using ERP.Application.Validators.Account.InputValidators.BaseValidators;
using ERP.Domain.Commands.Account;
using ERP.Domain.Models.Entities.Account.GLSettings;
using FluentValidation;

namespace ERP.Application.Validators.Account.InputValidators;

public class GLSettingInputValidator : BaseInputValidator<GLSettingInputModel>
{
    public GLSettingInputValidator() : base()
    {
        _ = RuleFor(e => e.DecimalDigitsNumber).GreaterThanOrEqualTo((byte)0).WithMessage("DecimalDigitsMINValue").LessThanOrEqualTo((byte)10).WithMessage("DecimalDigitsMAXValue");
        _ = RuleFor(e => e.MonthDays).InclusiveBetween((byte)1, (byte)31).When(e => e.DepreciationApplication.Equals(DepreciationApplication.Monthly)).WithMessage("MonthDaysMINValue");
        _ = RuleFor(e => e.MonthDays).Equal((byte)0).When(e => e.DepreciationApplication.Equals(DepreciationApplication.WithYearClosed)).WithMessage("MonthDaysMINValue");
    }
}