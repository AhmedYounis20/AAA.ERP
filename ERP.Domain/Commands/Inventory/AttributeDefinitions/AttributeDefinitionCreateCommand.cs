using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;
using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Commands.Inventory.AttributeValues;

namespace ERP.Domain.Commands.Inventory.AttributeDefinitions;

public class AttributeDefinitionCreateCommand : BaseSettingCreateCommand<AttributeDefinition>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameSecondLanguage { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public List<AttributeValueCreateCommand> PredefinedValues { get; set; } = new();
}
