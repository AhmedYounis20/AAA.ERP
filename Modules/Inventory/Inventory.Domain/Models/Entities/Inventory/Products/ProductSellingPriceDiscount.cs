using Shared.BaseEntities;
using Inventory.Domain.Models.Entities.Items;

namespace Inventory.Domain.Models.Entities.Products;

public class ProductSellingPriceDiscount : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public Guid SellingPriceId { get; set; }
    public decimal Discount { get; set; }
    public DiscountType DiscountType { get; set; }
}


