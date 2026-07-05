using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Models.Entities.Inventory.Attributes;

namespace Inventory.Application.Commands.AttributeValues;

public class AttributeValueInlineCreateCommand : BaseSettingCreateCommand<AttributeValue>
{
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
