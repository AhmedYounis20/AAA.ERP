using Shared.Domain.BaseEntities;

namespace Inventory.Domain.Models.Entities.Attributes;

public class AttributeValue : BaseSettingEntity
{
    public Guid AttributeDefinitionId { get; set; }
    public AttributeDefinition? AttributeDefinition { get; set; }
    public int? SortOrder { get; set; }
    public bool IsActive { get; set; }
}
