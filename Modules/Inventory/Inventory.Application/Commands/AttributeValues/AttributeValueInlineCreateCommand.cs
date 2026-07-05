using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Inventory.Domain.Models.Entities.Attributes;

namespace Inventory.Application.Commands.AttributeValues;

public class AttributeValueInlineCreateCommand : BaseSettingCreateCommand<AttributeValue>
{
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
