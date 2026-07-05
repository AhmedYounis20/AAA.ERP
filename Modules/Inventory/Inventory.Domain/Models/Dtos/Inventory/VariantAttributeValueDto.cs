using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Dtos;

public class VariantAttributeValueDto
{
    public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    public Guid AttributeDefinitionId { get; set; }
    public Guid AttributeValueId { get; set; }

    // Navigation properties
    public AttributeDefinitionDto? AttributeDefinition { get; set; }
    public AttributeValueDto? AttributeValue { get; set; }
    public string? AttributeValueHexCode { get; set; }
    public int? AttributeValueSortOrder { get; set; }

    public string? CreatedByName { get; set; }
    public string? CreatedByNameSecondLanguage { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? ModifiedByName { get; set; }
    public string? ModifiedByNameSecondLanguage { get; set; }
    public DateTime ModifiedAt { get; set; }
}



