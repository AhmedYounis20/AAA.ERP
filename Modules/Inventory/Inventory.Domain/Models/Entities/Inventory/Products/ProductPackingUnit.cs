using Shared.BaseEntities;

namespace Inventory.Domain.Models.Entities.Products;

public class ProductPackingUnit : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public Guid PackingUnitId { get; set; }
    public string? Name { get; set; }
    public int OrderNumber { get; set; }
    public int PartsCount { get; set; }
    public bool IsDefaultPackingUnit { get; set; }
    public bool IsDefaultSales { get; set; }
    public bool IsDefaultPurchases { get; set; }
    public decimal LastCostPrice { get; set; }
    public decimal AverageCostPrice { get; set; }
    
    // Navigation properties
    public List<ProductPackingUnitSellingPrice> ProductPackingUnitSellingPrices { get; set; } = [];
}


