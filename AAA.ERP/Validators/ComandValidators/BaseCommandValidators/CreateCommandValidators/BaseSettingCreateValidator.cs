using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using FluentValidation;

namespace Domain.Account.Validators.ComandValidators.BaseCommandValidators.CreateCommandValidators;

public class BaseSettingCreateValidator<TCommand,TResponse> : BaseCreateValidator<TCommand,TResponse>  where TCommand : BaseSettingCreateCommand<TResponse> 
{
    public BaseSettingCreateValidator()
    {
        _ = RuleFor(e => e.Name).NotEmpty().WithMessage("NameIsRequired").MaximumLength(50).WithMessage("NameMaxLength");
        _ = RuleFor(e => e.NameSecondLanguage).NotEmpty().WithMessage("NameSecondLanguageIsRequired").MaximumLength(50).WithMessage("NameSecondLanguageMaxLength");
    }
}