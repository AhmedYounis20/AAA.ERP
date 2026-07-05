namespace ERP.BuildingBlocks.Application.Commands.BaseInputModels;

public class BaseTreeSettingInputModel : BaseSettingInputModel
{
    public Guid? parentId { get; set; }
}