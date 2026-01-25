using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Models.Entities.Inventory.Attributes;

namespace ERP.Domain.Commands.Inventory.AttributeValues;

public class AttributeValueInlineCreateCommand : BaseSettingCreateCommand<AttributeValue>
{
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    // Note: No AttributeDefinitionId here; it is supplied by the parent definition
}


