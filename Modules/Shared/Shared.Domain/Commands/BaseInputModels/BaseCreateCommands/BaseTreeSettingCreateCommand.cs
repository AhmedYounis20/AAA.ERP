using Shared.Domain.BaseEntities;

namespace Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

public class BaseTreeSettingCreateCommand<TResponse> : BaseSettingCreateCommand<TResponse>
{
    public Guid? ParentId { get; set; }
    public NodeType NodeType { get; set; }

}