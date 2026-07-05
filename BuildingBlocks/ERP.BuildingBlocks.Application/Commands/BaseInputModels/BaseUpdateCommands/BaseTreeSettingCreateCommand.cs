using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseTreeSettingUpdateCommand<TResponse> : BaseSettingUpdateCommand<TResponse>
{
    public Guid? ParentId { get; set; }
    public NodeType NodeType { get; set; }
}