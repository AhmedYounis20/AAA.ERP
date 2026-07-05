using ERP.BuildingBlocks.Application.Commands.BaseInputModels;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;

public class BaseTreeSettingCreateCommand<TResponse> : BaseSettingCreateCommand<TResponse>
{
    public Guid? ParentId { get; set; }
    public NodeType NodeType { get; set; }

}