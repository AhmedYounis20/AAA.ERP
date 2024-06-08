using Domain.Account.Commands.FinancialPeriods;
using Domain.Account.Commands.GLSettings;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Models.Entities.GLSettings;
using Domain.Account.Validators.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using FluentValidation;

namespace Domain.Account.Validators.ComandValidators.GlSettings;

public class GlSettingUpdateValidator : BaseUpdateValidator<GlSettingUpdateCommand, GLSetting>
{
    public GlSettingUpdateValidator() : base()
    {
        _ = RuleFor(e => e.DecimalDigitsNumber).GreaterThanOrEqualTo((byte)0).WithMessage("DecimalDigitsMINValue").LessThanOrEqualTo((byte)10).WithMessage("DecimalDigitsMAXValue");
        _ = RuleFor(e => e.MonthDays).InclusiveBetween((byte)1, (byte)31).When(e => e.DepreciationApplication.Equals(DepreciationApplication.Monthly)).WithMessage("MonthDaysMINValue");
        _ = RuleFor(e => e.MonthDays).Equal((byte)0).When(e => e.DepreciationApplication.Equals(DepreciationApplication.WithYearClosed)).WithMessage("MonthDaysMINValue");
    }
}