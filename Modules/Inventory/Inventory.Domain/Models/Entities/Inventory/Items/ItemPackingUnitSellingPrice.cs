using Inventory.Domain.Models.Entities.SellingPrices;
using Shared.BaseEntities;

namespace Inventory.Domain.Models.Entities.Items;

public class ItemPackingUnitSellingPrice : BaseEntity
{
    public Guid ItemPackingUnitId { get; set; }
    public ItemPackingUnit? ItemPackingUnit { get; set; }
    public Guid SellingPriceId { get; set; }
    public SellingPrice? SellingPrice { get; set; }
    public decimal Amount { get; set; }
}