using Shared.BaseEntities;

namespace Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseTreeSettingUpdateCommand<TResponse> : BaseSettingUpdateCommand<TResponse>
{
    public Guid? ParentId { get; set; }
    public NodeType NodeType { get; set; }
}