namespace ERP.Domain.Models.Dtos.Inventory;

public class VariantCombinationDto 
{
    public string Name { get; set; } = string.Empty;
    public Dictionary<Guid, Guid> AttributeValues { get; set; } = [];
    public bool ApplyProductChanges { get; set; }
}
