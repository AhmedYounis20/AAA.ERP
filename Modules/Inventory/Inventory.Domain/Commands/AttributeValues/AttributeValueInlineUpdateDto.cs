using Inventory.Domain.Models.Entities.Attributes;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Inventory.Domain.Commands.AttributeValues;

public class AttributeValueInlineUpdateDto
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? NameSecondLanguage { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    // Note: No AttributeDefinitionId; it comes from the parent
}
