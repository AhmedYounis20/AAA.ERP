using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;

namespace Domain.Account.Validators.ComandValidators.BaseCommandValidators.CreateCommandValidators;

public class BaseTreeSettingCreateValidator<TCommand,TResponse> : BaseSettingCreateValidator<TCommand,TResponse> where TCommand : BaseTreeSettingCreateCommand<TResponse>
{
    public BaseTreeSettingCreateValidator() :base(){}
}