using Inventory.Domain.Commands.AttributeValues;
using Inventory.Domain.Models.Entities.Attributes;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Inventory.Domain.Commands.AttributeDefinitions;

public class AttributeDefinitionUpdateCommand : BaseSettingUpdateCommand<AttributeDefinition>
{
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public List<AttributeValueInlineUpdateDto> PredefinedValues { get; set; } = new();
}
