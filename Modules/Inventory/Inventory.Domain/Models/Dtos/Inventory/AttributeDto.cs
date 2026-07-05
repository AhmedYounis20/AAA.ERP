using Shared.BaseEntities;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Domain.Models.Dtos.Inventory;

public class AttributeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? NameSecondLanguage { get; set; }
    public string? Description { get; set; }
    public AttributeDataType DataType { get; set; }
    public bool IsRequired { get; set; }
    public bool IsSearchable { get; set; }
    public int SortOrder { get; set; }
    public List<AttributeValueDto> AttributeValues { get; set; } = [];

    public string? CreatedByName { get; set; }
    public string? CreatedByNameSecondLanguage { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? ModifiedByName { get; set; }
    public string? ModifiedByNameSecondLanguage { get; set; }
    public DateTime ModifiedAt { get; set; }
}
