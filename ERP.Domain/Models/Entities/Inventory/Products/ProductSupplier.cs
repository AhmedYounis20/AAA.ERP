using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Products;

public class ProductSupplier : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid SupplierId { get; set; }
}