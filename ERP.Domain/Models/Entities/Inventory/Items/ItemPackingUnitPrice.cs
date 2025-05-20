using ERP.Domain.Models.Entities.Inventory.PackingUnits;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Items;

public class ItemPackingUnitPrice : BaseEntity
{
    public Guid ItemId { get; set; }
    public Guid PackingUnitId { get; set; }
    public PackingUnit? PackingUnit { get; set; }
    public int PartsCount { get; set; }
    public bool IsDefaultSales { get; set; }
    public bool IsDefaultPurchases { get; set; }
    public decimal LastCostPrice { get; set; }
    public decimal AverageCostPrice { get; set; }
    public List<ItemPackingUnitSellingPrice> ItemPackingUnitSellingPrices { get; set; } = [];
}