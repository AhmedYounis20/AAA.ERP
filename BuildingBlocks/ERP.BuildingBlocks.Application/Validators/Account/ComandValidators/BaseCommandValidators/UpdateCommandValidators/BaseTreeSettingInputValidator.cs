using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

namespace ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;

public class BaseTreeSettingUpdateValidator<TCommand, TResponse> : BaseSettingUpdateValidator<TCommand, TResponse> where TCommand : BaseTreeSettingUpdateCommand<TResponse>
{
    public BaseTreeSettingUpdateValidator() : base() { }
}