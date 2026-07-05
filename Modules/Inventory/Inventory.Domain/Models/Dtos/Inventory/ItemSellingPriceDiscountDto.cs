using Inventory.Domain.Models.Entities.Items;

namespace Inventory.Domain.Models.Dtos;

public class ItemSellingPriceDiscountDto
{
    public Guid SellingPriceId { get; set; }
    public decimal Discount { get; set; }
    public DiscountType DiscountType { get; set; }
}