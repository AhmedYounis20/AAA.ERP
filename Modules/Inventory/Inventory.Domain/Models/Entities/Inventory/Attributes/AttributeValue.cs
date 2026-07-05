using ERP.Domain.Models.Entities.Inventory.Variants;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Attributes;

public class AttributeValue : BaseSettingEntity
{
    public Guid AttributeDefinitionId { get; set; }
    public AttributeDefinition? AttributeDefinition { get; set; }
    public int? SortOrder { get; set; }
    public bool IsActive { get; set; }
    public ICollection<VariantAttributeValue> VariantAttributeValues { get; set; } = new List<VariantAttributeValue>();
}