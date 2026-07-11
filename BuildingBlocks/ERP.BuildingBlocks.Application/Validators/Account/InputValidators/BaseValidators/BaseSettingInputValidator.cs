using ERP.BuildingBlocks.Application.Commands.BaseInputModels;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace ERP.BuildingBlocks.Application.Validators.Account.InputValidators.BaseValidators;

public class BaseSettingInputValidator<TEntity> : BaseInputValidator<TEntity> where TEntity : BaseSettingInputModel
{
    public BaseSettingInputValidator()
    {
        _ = RuleFor(e => e.Name).NotEmpty().WithMessage(ValidationMessages.NameIsRequired).MaximumLength(50).WithMessage(ValidationMessages.NameMaxLength);
        _ = RuleFor(e => e.NameSecondLanguage).NotEmpty().WithMessage(ValidationMessages.NameSecondLanguageIsRequired).MaximumLength(50).WithMessage(ValidationMessages.NameSecondLanguageMaxLength);
    }
}