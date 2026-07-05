using ERP.BuildingBlocks.Application.Commands.BaseInputModels;

namespace ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;

public class BaseTreeCreateCommand<TResponse> : BaseCreateCommand<TResponse>
{
    public Guid? ParentId { get; set; }
}