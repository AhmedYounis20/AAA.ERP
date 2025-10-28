using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Account.Application.Validators.CommandValidators.BaseCommandValidators.CreateCommandValidators;

public class BaseTreeSettingCreateValidator<TCommand, TResponse> : BaseSettingCreateValidator<TCommand, TResponse> where TCommand : BaseTreeSettingCreateCommand<TResponse>
{
    public BaseTreeSettingCreateValidator() : base() { }
}