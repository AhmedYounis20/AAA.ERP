using Shared.Domain.BaseEntities;

namespace Inventory.Domain.Models.Entities.Attributes;

public class AttributeDefinition : BaseSettingEntity
{
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    // Navigation properties
    public ICollection<AttributeValue> PredefinedValues { get; set; } = new List<AttributeValue>();
}
