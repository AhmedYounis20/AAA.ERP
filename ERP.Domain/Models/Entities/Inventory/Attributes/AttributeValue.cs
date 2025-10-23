using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

public class AttributeValue : BaseSettingEntity
{
    public Guid AttributeDefinitionId { get; set; }
    public AttributeDefinition? AttributeDefinition { get; set; }
    public int? SortOrder { get; set; }
    public bool IsActive { get; set; }
}