using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Products;

public class ProductPurchaseTax : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid TaxId { get; set; }
}
