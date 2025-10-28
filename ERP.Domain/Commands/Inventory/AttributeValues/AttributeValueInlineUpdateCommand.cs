using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Domain.Commands.Inventory.AttributeValues;

public class AttributeValueInlineUpdateCommand : BaseSettingUpdateCommand<AttributeValue>
{
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    // Note: No AttributeDefinitionId; it comes from the parent
}