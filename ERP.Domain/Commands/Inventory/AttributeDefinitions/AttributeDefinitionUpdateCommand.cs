using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Commands.Inventory.AttributeValues;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Domain.Commands.Inventory.AttributeDefinitions;

public class AttributeDefinitionUpdateCommand : BaseSettingUpdateCommand<AttributeDefinition>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public List<AttributeValueInlineUpdateDto> PredefinedValues { get; set; } = new();
}
