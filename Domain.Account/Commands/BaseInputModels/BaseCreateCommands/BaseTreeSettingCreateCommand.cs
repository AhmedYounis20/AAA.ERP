using Domain.Account.InputModels.BaseInputModels;

namespace Domain.Account.Commands.BaseInputModels.BaseCreateCommands;

public class BaseTreeSettingCreateCommand<TResponse> : BaseSettingCreateCommand<TResponse>
{
    public Guid? ParentId { get; set; }
}