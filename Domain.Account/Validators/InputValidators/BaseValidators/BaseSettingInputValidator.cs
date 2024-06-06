using Domain.Account.InputModels.BaseInputModels;
using FluentValidation;

namespace AAA.ERP.Validators.InputValidators.BaseValidators;

public class BaseSettingInputValidator<TEntity> : BaseInputValidator<TEntity> where TEntity : BaseSettingInputModel
{
    public BaseSettingInputValidator()
    {
        _ = RuleFor(e => e.Name).NotEmpty().WithMessage("NameIsRequired").MaximumLength(50).WithMessage("NameMaxLength");
        _ = RuleFor(e => e.NameSecondLanguage).NotEmpty().WithMessage("NameSecondLanguageIsRequired").MaximumLength(50).WithMessage("NameSecondLanguageMaxLength");
    }
}