namespace Shared.Domain.Commands.BaseInputModels;

public class BaseTreeInputModel : BaseInputModel
{
    public Guid? parentId { get; set; }
}