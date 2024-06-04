using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using FluentValidation;

namespace Domain.Account.Validators.ComandValidators.BaseCommandValidators.UpdateCommandValidators;

public class BaseSettingUpdateValidator<TCommand,TResponse> : BaseUpdateValidator<TCommand,TResponse>  where TCommand : BaseSettingUpdateCommand<TResponse> 
{
    public BaseSettingUpdateValidator()
    {
        _ = RuleFor(e => e.Name).NotEmpty().WithMessage("NameIsRequired").MaximumLength(50).WithMessage("NameMaxLength");
        _ = RuleFor(e => e.NameSecondLanguage).NotEmpty().WithMessage("NameSecondLanguageIsRequired").MaximumLength(50).WithMessage("NameSecondLanguageMaxLength");
    }
}