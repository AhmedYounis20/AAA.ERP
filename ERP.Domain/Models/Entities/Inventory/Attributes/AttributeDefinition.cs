using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

public class AttributeDefinition : BaseSettingEntity
{
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    // Navigation properties
    public ICollection<AttributeValue> PredefinedValues { get; set; } = new List<AttributeValue>();
}