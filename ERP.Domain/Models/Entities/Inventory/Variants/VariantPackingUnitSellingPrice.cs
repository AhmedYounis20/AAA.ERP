using ERP.Domain.Models.Entities.Inventory.SellingPrices;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Variants;

public class VariantPackingUnitSellingPrice : BaseEntity
{
    public Guid VariantPackingUnitId { get; set; }
    public VariantPackingUnit? VariantPackingUnit { get; set; }
    public Guid SellingPriceId { get; set; }
    public SellingPrice? SellingPrice { get; set; }
    public decimal Amount { get; set; }
}