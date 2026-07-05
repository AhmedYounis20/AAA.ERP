namespace ERP.BuildingBlocks.Application.Commands.BaseInputModels;

public class BaseTreeInputModel : BaseInputModel
{
    public Guid? parentId { get; set; }
}