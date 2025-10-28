using Shared.Domain.BaseEntities;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Shared.Application.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseTreeSettingUpdateCommand<TResponse> : BaseSettingUpdateCommand<TResponse>
{
    public Guid? ParentId { get; set; }
    public NodeType NodeType { get; set; }
}