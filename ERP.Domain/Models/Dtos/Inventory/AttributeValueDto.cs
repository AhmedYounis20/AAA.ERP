using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Domain.Models.Dtos.Inventory;

public class AttributeValueDto
{
    public Guid Id { get; set; }
    public Guid AttributeDefinitionId { get; set; }
    public string? Name { get; set; } = string.Empty;
    public string? NameSecondLanguage { get; set; } = string.Empty;
    public int? SortOrder { get; set; }
    public bool IsActive { get; set; }
    public string? AttributeDefinitionName { get; set; }
    public string? AttributeDefinitionNameSecondLanguange { get; set; }
}