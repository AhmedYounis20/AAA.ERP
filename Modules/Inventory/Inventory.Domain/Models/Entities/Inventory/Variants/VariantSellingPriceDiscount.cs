using Inventory.Domain.Models.Entities;
using Inventory.Domain.Models.Entities.SellingPrices;
using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Variants;

public class VariantSellingPriceDiscount : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid SellingPriceId { get; set; }
    public SellingPrice? SellingPrice { get; set; }
    public decimal Discount { get; set; }
    public DiscountType DiscountType { get; set; }
}