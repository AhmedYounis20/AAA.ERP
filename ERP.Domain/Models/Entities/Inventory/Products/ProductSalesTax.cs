using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Products;

public class ProductSalesTax : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid TaxId { get; set; }
}
