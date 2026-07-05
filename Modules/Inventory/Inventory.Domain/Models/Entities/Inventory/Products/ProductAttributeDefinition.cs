using Inventory.Domain.Models.Entities.AttributeDefinitions;
using Inventory.Domain.Models.Entities.Attributes;
using Shared.BaseEntities;

namespace Inventory.Domain.Models.Entities.Products;

public class ProductAttributeDefinition : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid AttributeDefinitionId { get; set; }
    public AttributeDefinition? AttributeDefinition { get; set; }
    public bool IsVariant { get; set; }
}





