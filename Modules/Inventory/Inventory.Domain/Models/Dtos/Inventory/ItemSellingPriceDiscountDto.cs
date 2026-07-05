namespace ERP.Domain.Models.Entities.Inventory.Items;

public class ItemSellingPriceDiscountDto
{
    public Guid SellingPriceId { get; set; }
    public decimal Discount { get; set; }
    public DiscountType DiscountType { get; set; }
}