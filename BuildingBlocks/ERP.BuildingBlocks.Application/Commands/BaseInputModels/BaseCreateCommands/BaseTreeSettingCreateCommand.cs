using Domain.Account.InputModels.BaseInputModels;
using Shared.BaseEntities;

namespace Domain.Account.Commands.BaseInputModels.BaseCreateCommands;

public class BaseTreeSettingCreateCommand<TResponse> : BaseSettingCreateCommand<TResponse>
{
    public Guid? ParentId { get; set; }
    public NodeType NodeType { get; set; }

}