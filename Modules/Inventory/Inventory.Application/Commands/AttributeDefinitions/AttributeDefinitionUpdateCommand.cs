using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Inventory.Domain.Models.Entities.Attributes;
using Inventory.Application.Commands.AttributeValues;

namespace Inventory.Application.Commands.AttributeDefinitions;

public class AttributeDefinitionUpdateCommand : BaseSettingUpdateCommand<AttributeDefinition>
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public List<AttributeValueInlineUpdateDto> PredefinedValues { get; set; } = [];
}
