namespace Account.Application.Validators.CommandValidators.BaseCommandValidators.UpdateCommandValidators;

public class BaseTreeSettingUpdateValidator<TCommand, TResponse> : BaseSettingUpdateValidator<TCommand, TResponse> where TCommand : BaseTreeSettingUpdateCommand<TResponse>
{
    public BaseTreeSettingUpdateValidator() : base() { }
}