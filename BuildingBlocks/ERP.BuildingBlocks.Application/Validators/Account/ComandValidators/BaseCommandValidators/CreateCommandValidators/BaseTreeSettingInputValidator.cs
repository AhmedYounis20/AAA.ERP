using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;

namespace ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;

public class BaseTreeSettingCreateValidator<TCommand, TResponse> : BaseSettingCreateValidator<TCommand, TResponse> where TCommand : BaseTreeSettingCreateCommand<TResponse>
{
    public BaseTreeSettingCreateValidator() : base() { }
}