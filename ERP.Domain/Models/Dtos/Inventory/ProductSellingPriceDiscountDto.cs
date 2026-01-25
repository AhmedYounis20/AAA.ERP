using ERP.Domain.Models.Entities.Inventory.Items;

namespace ERP.Domain.Models.Dtos.Inventory;

public class ProductSellingPriceDiscountDto
{
    public Guid SellingPriceId { get; set; }
    public decimal Discount { get; set; }
    public DiscountType DiscountType { get; set; }
}
