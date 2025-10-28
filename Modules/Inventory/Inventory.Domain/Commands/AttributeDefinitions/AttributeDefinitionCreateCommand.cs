using Inventory.Domain.Commands.AttributeValues;
using Inventory.Domain.Models.Entities.Attributes;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Inventory.Domain.Commands.AttributeDefinitions;

public class AttributeDefinitionCreateCommand : BaseSettingCreateCommand<AttributeDefinition>
{
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public List<AttributeValueCreateCommand> PredefinedValues { get; set; } = new();
}
