using ERP.Domain.Models.Entities.Inventory.SellingPrices;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Items;

public class ItemPackingUnitSellingPrice : BaseEntity
{
    public Guid ItemPackingUnitId { get; set; }
    public ItemPackingUnit? ItemPackingUnit { get; set; }
    public Guid SellingPriceId { get; set; }
    public SellingPrice? SellingPrice { get; set; }
    public decimal Amount { get; set; }
}