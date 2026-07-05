using Inventory.Domain.Models.Entities.PackingUnits;
using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Items;

public class ItemPackingUnit : BaseEntity
{
    public Guid ItemId { get; set; }
    public Item? Item { get; set; }
    public Guid PackingUnitId { get; set; }
    public PackingUnit? PackingUnit { get; set; }
    public int PartsCount { get; set; }
    public bool IsDefaultPackingUnit { get; set; }
    public bool IsDefaultSales { get; set; }
    public bool IsDefaultPurchases { get; set; }
    public decimal LastCostPrice { get; set; }
    public decimal AverageCostPrice { get; set; }
    public int OrderNumber { get; set; }
    public List<ItemPackingUnitSellingPrice> ItemPackingUnitSellingPrices { get; set; } = [];
}