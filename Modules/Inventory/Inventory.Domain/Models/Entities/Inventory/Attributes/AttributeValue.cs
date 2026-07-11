using Inventory.Domain.Models.Entities.Variants;
using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Attributes;

public class AttributeValue : BaseSettingEntity
{
    public Guid AttributeDefinitionId { get; set; }
    public AttributeDefinition? AttributeDefinition { get; set; }
    public int? SortOrder { get; set; }
    public bool IsActive { get; set; }
    public ICollection<VariantAttributeValue> VariantAttributeValues { get; set; } = new List<VariantAttributeValue>();
}