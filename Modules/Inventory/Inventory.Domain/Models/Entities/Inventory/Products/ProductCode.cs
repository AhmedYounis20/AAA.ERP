using Shared.BaseEntities;

namespace Inventory.Domain.Models.Entities.Products;

public class ProductCode : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public ProductCodeType CodeType { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
}