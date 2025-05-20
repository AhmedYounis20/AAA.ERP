using ERP.Domain.Models.Entities.Inventory.SellingPrices;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Items;

public class ItemSellingPriceDiscount : BaseEntity
{
    public Guid ItemId { get; set; }
    public Guid SellingPriceId { get; set; }
    public SellingPrice? SellingPrice { get; set; }
    public decimal Discount { get; set; }
    public DiscountType DiscountType { get; set; }
}