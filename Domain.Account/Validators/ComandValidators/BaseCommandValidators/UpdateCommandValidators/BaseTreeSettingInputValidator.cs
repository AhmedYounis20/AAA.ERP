using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

namespace Domain.Account.Validators.ComandValidators.BaseCommandValidators.UpdateCommandValidators;

public class BaseTreeSettingUpdateValidator<TCommand,TResponse> : BaseSettingUpdateValidator<TCommand,TResponse> where TCommand : BaseTreeSettingUpdateCommand<TResponse>
{
    public BaseTreeSettingUpdateValidator() :base(){}
}