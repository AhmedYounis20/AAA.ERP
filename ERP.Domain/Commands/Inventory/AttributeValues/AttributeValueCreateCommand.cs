using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Domain.Commands.Inventory.AttributeValues;

public class AttributeValueCreateCommand : BaseSettingCreateCommand<AttributeValue>
{
    public string Value { get; set; } = string.Empty;
    public string? ValueSecondLanguage { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid AttributeDefinitionId { get; set; }
}
