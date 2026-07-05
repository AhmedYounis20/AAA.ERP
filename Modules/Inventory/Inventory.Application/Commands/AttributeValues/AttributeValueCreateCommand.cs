using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Models.Entities.Inventory.Attributes;

namespace Inventory.Application.Commands.AttributeValues;

public class AttributeValueCreateCommand : BaseSettingCreateCommand<AttributeValue>
{
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid AttributeDefinitionId { get; set; }
}
