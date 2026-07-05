using ERP.BuildingBlocks.Application.Validators.Account.InputValidators.BaseValidators;
using Accounting.Application.Commands;
using Accounting.Domain.Models.Entities.GLSettings;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Application.Validators.GLSettings;

public class GLSettingInputValidator : BaseInputValidator<GLSettingInputModel>
{
    public GLSettingInputValidator() : base()
    {
        _ = RuleFor(e => e.DecimalDigitsNumber).GreaterThanOrEqualTo((byte)0).WithMessage(ValidationMessages.DecimalDigitsMinValue).LessThanOrEqualTo((byte)10).WithMessage(ValidationMessages.DecimalDigitsMaxValue);
        _ = RuleFor(e => e.MonthDays).InclusiveBetween((byte)1, (byte)31).When(e => e.DepreciationApplication.Equals(DepreciationApplication.Monthly)).WithMessage(ValidationMessages.MonthDaysMinValue);
    }
}
