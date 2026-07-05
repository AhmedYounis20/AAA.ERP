using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Products;

public class ProductCode : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public ProductCodeType CodeType { get; set; }
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
}