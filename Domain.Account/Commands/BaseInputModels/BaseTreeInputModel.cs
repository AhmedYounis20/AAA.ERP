namespace Domain.Account.InputModels.BaseInputModels;

public class BaseTreeInputModel : BaseInputModel
{
    public Guid? parentId { get; set; }
}