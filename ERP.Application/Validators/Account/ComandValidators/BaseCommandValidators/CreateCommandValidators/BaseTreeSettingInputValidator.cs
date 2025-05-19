using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;

namespace ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;

public class BaseTreeSettingCreateValidator<TCommand, TResponse> : BaseSettingCreateValidator<TCommand, TResponse> where TCommand : BaseTreeSettingCreateCommand<TResponse>
{
    public BaseTreeSettingCreateValidator() : base() { }
}