namespace ERP.Domain.Models.Dtos.Inventory;

public class ProductPackingUnitDto
{
    public Guid PackingUnitId { get; set; }
    public string? Name { get; set; }
    public int OrderNumber { get; set; }
    public int PartsCount { get; set; }
    public bool IsDefaultPackingUnit { get; set; }
    public bool IsDefaultSales { get; set; }
    public bool IsDefaultPurchases { get; set; }
    public decimal LastCostPrice { get; set; }
    public decimal AverageCostPrice { get; set; }
    public List<ProductPackingUnitSellingPriceDto> SellingPrices { get; set; } = [];
}


