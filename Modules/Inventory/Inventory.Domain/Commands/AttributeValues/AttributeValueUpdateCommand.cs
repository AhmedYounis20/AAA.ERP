using Inventory.Domain.Models.Entities.Attributes;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Inventory.Domain.Commands.AttributeValues;

public class AttributeValueUpdateCommand : BaseSettingUpdateCommand<AttributeValue>
{
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid AttributeDefinitionId { get; set; }
}
