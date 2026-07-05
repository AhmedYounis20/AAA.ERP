using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.UpdateCommandValidators;

public class BaseSettingUpdateValidator<TCommand, TResponse> : BaseUpdateValidator<TCommand, TResponse> where TCommand : BaseSettingUpdateCommand<TResponse>
{
    public BaseSettingUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Name).NotEmpty().WithMessage(ValidationMessages.NameIsRequired).MaximumLength(50).WithMessage(ValidationMessages.NameMaxLength);
        _ = RuleFor(e => e.NameSecondLanguage).NotEmpty().WithMessage(ValidationMessages.NameSecondLanguageIsRequired).MaximumLength(50).WithMessage(ValidationMessages.NameSecondLanguageMaxLength);
    }
}