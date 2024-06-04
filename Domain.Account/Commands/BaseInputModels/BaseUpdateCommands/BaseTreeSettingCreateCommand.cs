namespace Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseTreeSettingUpdateCommand<TCommand> : BaseSettingUpdateCommand<TCommand>
{
    public Guid? ParentId { get; set; }
}