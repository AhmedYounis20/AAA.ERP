using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Products;

public class ProductCostCenter : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid CostCenterId { get; set; }
    public decimal Percent { get; set; }
}
