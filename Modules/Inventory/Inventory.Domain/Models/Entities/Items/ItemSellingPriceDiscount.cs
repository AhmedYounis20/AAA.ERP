using Inventory.Domain.Models.Entities.SellingPrices;
using Shared.Domain.BaseEntities;

namespace Inventory.Domain.Models.Entities.Items;

public class ItemSellingPriceDiscount : BaseEntity
{
    public Guid ItemId { get; set; }
    public Item? Item { get; set; }
    public Guid SellingPriceId { get; set; }
    public SellingPrice? SellingPrice { get; set; }
    public decimal Discount { get; set; }
    public DiscountType DiscountType { get; set; }
}
