using FluentValidation;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Account.Application.Validators.CommandValidators.BaseCommandValidators.UpdateCommandValidators;

public class BaseSettingUpdateValidator<TCommand, TResponse> : BaseUpdateValidator<TCommand, TResponse> where TCommand : BaseSettingUpdateCommand<TResponse>
{
    public BaseSettingUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Name).NotEmpty().WithMessage("NameIsRequired").MaximumLength(50).WithMessage("NameMaxLength");
        _ = RuleFor(e => e.NameSecondLanguage).NotEmpty().WithMessage("NameSecondLanguageIsRequired").MaximumLength(50).WithMessage("NameSecondLanguageMaxLength");
    }
}