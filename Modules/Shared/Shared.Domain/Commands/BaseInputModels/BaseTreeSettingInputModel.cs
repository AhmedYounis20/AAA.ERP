namespace Shared.Domain.Commands.BaseInputModels;

public class BaseTreeSettingInputModel : BaseSettingInputModel
{
    public Guid? parentId { get; set; }
}