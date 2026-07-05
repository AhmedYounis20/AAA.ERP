using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Domain.Models.Dtos.Inventory;

public class AttributeDefinitionDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? NameSecondLanguage { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    public List<AttributeValueDto> PredefinedValues { get; set; } = new();
}





