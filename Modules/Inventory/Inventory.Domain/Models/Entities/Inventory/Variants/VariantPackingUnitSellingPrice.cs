using Inventory.Domain.Models.Entities.SellingPrices;
using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Variants;

public class VariantPackingUnitSellingPrice : BaseEntity
{
    public Guid VariantPackingUnitId { get; set; }
    public VariantPackingUnit? VariantPackingUnit { get; set; }
    public Guid SellingPriceId { get; set; }
    public SellingPrice? SellingPrice { get; set; }
    public decimal Amount { get; set; }
}