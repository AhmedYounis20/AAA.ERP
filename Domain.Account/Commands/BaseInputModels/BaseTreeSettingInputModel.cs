namespace Domain.Account.InputModels.BaseInputModels;

public class BaseTreeSettingInputModel : BaseSettingInputModel
{
    public Guid? parentId { get; set; }
}