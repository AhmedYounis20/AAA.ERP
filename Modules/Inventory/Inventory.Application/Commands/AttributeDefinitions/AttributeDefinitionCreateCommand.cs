using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using Inventory.Domain.Models.Entities.Attributes;
using Inventory.Application.Commands.AttributeValues;

namespace Inventory.Application.Commands.AttributeDefinitions;

public class AttributeDefinitionCreateCommand : BaseSettingCreateCommand<AttributeDefinition>
{
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public List<AttributeValueInlineCreateCommand> PredefinedValues { get; set; } = [];
}
