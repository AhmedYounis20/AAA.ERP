
using Inventory.Domain.Models.Entities.Attributes;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Inventory.Domain.Commands.AttributeValues;

public class AttributeValueCreateCommand : BaseSettingCreateCommand<AttributeValue>
{
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid AttributeDefinitionId { get; set; }
}
