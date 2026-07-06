using Inventory.Domain.Models.Entities;

namespace Inventory.Application.Dtos;

public class ProductSellingPriceDiscountDto
{
    public Guid SellingPriceId { get; set; }
    public decimal Discount { get; set; }
    public DiscountType DiscountType { get; set; }
}
