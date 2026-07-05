using Inventory.Domain.Models.Entities.SellingPrices;
using ERP.BuildingBlocks.Domain.Entities;

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