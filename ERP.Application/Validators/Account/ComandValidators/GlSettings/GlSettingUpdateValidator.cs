using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using ERP.Domain.Commands.Account.GLSettings;
using ERP.Domain.Models.Entities.Account.GLSettings;
using FluentValidation;

namespace ERP.Application.Validators.Account.ComandValidators.GlSettings;

public class GlSettingUpdateValidator : BaseUpdateValidator<GlSettingUpdateCommand, GLSetting>
{
    public GlSettingUpdateValidator() : base()
    {
        _ = RuleFor(e => e.DecimalDigitsNumber).GreaterThanOrEqualTo((byte)0).WithMessage("DecimalDigitsMINValue").LessThanOrEqualTo((byte)10).WithMessage("DecimalDigitsMAXValue");
        _ = RuleFor(e => e.MonthDays).InclusiveBetween((byte)1, (byte)31).When(e => e.DepreciationApplication.Equals(DepreciationApplication.Monthly)).WithMessage("MonthDaysMINValue");
    }
}