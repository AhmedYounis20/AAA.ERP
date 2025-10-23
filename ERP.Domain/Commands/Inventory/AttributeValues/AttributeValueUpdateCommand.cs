using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Domain.Commands.Inventory.AttributeValues;

public class AttributeValueUpdateCommand : BaseSettingUpdateCommand<AttributeValue>
{
    public string Value { get; set; } = string.Empty;
    public string? ValueSecondLanguage { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid AttributeDefinitionId { get; set; }
}
