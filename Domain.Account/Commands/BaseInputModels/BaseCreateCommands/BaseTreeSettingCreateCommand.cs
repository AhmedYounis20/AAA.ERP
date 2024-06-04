using Domain.Account.InputModels.BaseInputModels;

namespace Domain.Account.Commands.BaseInputModels.BaseCreateCommands;

public class BaseTreeSettingInputModel<TCommand> : BaseSettingInputModel<TCommand>
{
    public Guid? ParentId { get; set; }
}