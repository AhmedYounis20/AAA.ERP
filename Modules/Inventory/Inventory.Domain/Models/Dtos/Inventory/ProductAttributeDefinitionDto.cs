namespace Inventory.Domain.Models.Dtos;

public class ProductAttributeDefinitionDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid AttributeDefinitionId { get; set; }
    public AttributeDefinitionDto? AttributeDefinition { get; set; }
    public bool IsVariant { get; set; }
}


