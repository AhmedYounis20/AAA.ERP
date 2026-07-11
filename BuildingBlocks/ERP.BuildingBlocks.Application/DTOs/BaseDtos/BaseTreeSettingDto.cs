namespace ERP.BuildingBlocks.Application.Dtos.BaseDtos;

public class BaseTreeSettingDto : BaseDto
{
    public Guid? ParentId { get; set; }
    public List<BaseTreeDto> Children { get; set; } = new();

    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
}