namespace ERP.BuildingBlocks.Application.Dtos.BaseDtos;

public class BaseTreeDto : BaseDto
{
    public Guid? ParentId { get; set; }
    public List<BaseTreeDto> Children { get; set; } = new();
}