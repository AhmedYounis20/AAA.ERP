using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;
using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Commands.Inventory.AttributeValues;
using ERP.Domain.Models.Entities.Inventory.Attributes;

namespace ERP.Domain.Commands.Inventory.AttributeDefinitions;

public class AttributeDefinitionCreateCommand : BaseSettingCreateCommand<AttributeDefinition>
{
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public List<AttributeValueInlineCreateCommand> PredefinedValues { get; set; } = new();
}
