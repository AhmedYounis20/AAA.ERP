namespace ERP.Domain.Models.Entities.Inventory.Items;

public class ItemPackingUnitDto
{
    public Guid PackingUnitId { get; set; }
    public int PartsCount { get; set; }
    public bool IsDefaultPackingUnit { get; set; }
    public bool IsDefaultSales { get; set; }
    public bool IsDefaultPurchases { get; set; }
    public decimal LastCostPrice { get; set; }
    public decimal AverageCostPrice { get; set; }
    public int OrderNumber { get; set; }
    public List<ItemPackingUnitSellingPriceDto> SellingPrices { get; set; } = [];
}