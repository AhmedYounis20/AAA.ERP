namespace ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseTreeUpdateCommand<TResponse> : BaseUpdateCommand<TResponse>
{
    public Guid? ParentId { get; set; }
}