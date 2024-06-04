using Domain.Account.InputModels.BaseInputModels;

namespace Domain.Account.Commands.BaseInputModels.BaseCreateCommands;

public class BaseTreeSettingCreateCommand<TCommand> : BaseSettingCreateCommand<TCommand>
{
    public Guid? ParentId { get; set; }
}