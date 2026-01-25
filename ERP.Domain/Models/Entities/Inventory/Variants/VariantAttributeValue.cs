using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;
using ERP.Domain.Models.Entities.Inventory.Attributes;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Variants;

public class VariantAttributeValue : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid AttributeDefinitionId { get; set; } 
    public AttributeDefinition? AttributeDefinition { get; set; }
    public Guid AttributeValueId { get; set; }
    public AttributeValue? AttributeValue { get; set; }  // red // xl / ls
}





