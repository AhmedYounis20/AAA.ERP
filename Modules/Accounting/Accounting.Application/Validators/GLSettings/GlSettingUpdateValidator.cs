using Accounting.Application.Commands.GLSettings;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Accounting.Domain.Models.Entities.GLSettings;
using FluentValidation;

namespace Accounting.Application.Validators.GLSettings;

public class GlSettingUpdateValidator : BaseUpdateValidator<GlSettingUpdateCommand, GLSetting>
{
    public GlSettingUpdateValidator() : base()
    {
        _ = RuleFor(e => e.DecimalDigitsNumber).GreaterThanOrEqualTo((byte)0).WithMessage("DecimalDigitsMINValue").LessThanOrEqualTo((byte)10).WithMessage("DecimalDigitsMAXValue");
        _ = RuleFor(e => e.MonthDays).InclusiveBetween((byte)1, (byte)31).When(e => e.DepreciationApplication.Equals(DepreciationApplication.Monthly)).WithMessage("MonthDaysMINValue");
    }
}
