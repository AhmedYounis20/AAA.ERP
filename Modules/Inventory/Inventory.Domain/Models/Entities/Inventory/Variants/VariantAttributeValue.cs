using Inventory.Domain.Models.Entities.AttributeDefinitions;
using Inventory.Domain.Models.Entities.Attributes;
using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Variants;

public class VariantAttributeValue : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid AttributeDefinitionId { get; set; }
    public AttributeDefinition? AttributeDefinition { get; set; }
    public Guid AttributeValueId { get; set; }
    public AttributeValue? AttributeValue { get; set; }  // red // xl / ls
}





