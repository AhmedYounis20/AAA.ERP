using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using FluentValidation;
using Shared.Responses;
using ERP.BuildingBlocks.Application.Constants;

namespace ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;

public class BaseSettingCreateValidator<TCommand, TResponse> : BaseCreateValidator<TCommand, TResponse> where TCommand : BaseSettingCreateCommand<TResponse>
{
    public BaseSettingCreateValidator() : base()
    {
        _ = RuleFor(e => e.Name).NotEmpty().WithMessage(ValidationMessages.NameIsRequired).MaximumLength(50).WithMessage(ValidationMessages.NameMaxLength);
        _ = RuleFor(e => e.NameSecondLanguage).NotEmpty().WithMessage(ValidationMessages.NameSecondLanguageIsRequired).MaximumLength(50).WithMessage(ValidationMessages.NameSecondLanguageMaxLength);
    }
}