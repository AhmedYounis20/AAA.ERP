using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Application.Commands.AttributeValues;

public class AttributeValueUpdateCommand : BaseSettingUpdateCommand<AttributeValue>
{
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid AttributeDefinitionId { get; set; }
}
