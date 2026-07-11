using Inventory.Domain.Models.Entities.PackingUnits;
using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Variants;

public class VariantPackingUnit : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid PackingUnitId { get; set; }
    public PackingUnit? PackingUnit { get; set; }
    public decimal PartsCount { get; set; }
    public bool IsDefaultPackingUnit { get; set; }
    public bool IsDefaultSales { get; set; }
    public bool IsDefaultPurchases { get; set; }
    public decimal LastCostPrice { get; set; }
    public decimal AverageCostPrice { get; set; }
    public int OrderNumber { get; set; }
    public ICollection<VariantPackingUnitSellingPrice> VariantPackingUnitSellingPrices { get; set; } = new List<VariantPackingUnitSellingPrice>();
}