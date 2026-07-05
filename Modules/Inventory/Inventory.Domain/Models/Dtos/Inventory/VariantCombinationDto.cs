namespace Inventory.Domain.Models.Dtos;

public class VariantCombinationDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Dictionary<Guid, Guid> AttributeValues { get; set; } = [];
    public bool ApplyProductChanges { get; set; }
}
