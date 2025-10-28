using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Commands.Inventory.AttributeValues;
using ERP.Domain.Models.Entities.Inventory.AttributeDefinitions;

namespace ERP.Domain.Commands.Inventory.AttributeDefinitions;

public class AttributeDefinitionUpdateCommand : BaseSettingUpdateCommand<AttributeDefinition>
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameSecondLanguage { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public List<AttributeValueInlineUpdateCommand> PredefinedValues { get; set; } = new();
}
