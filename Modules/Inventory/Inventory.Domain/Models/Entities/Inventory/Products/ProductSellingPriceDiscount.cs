using Shared.BaseEntities;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace ERP.Domain.Models.Entities.Inventory.Products;

public class ProductSellingPriceDiscount : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public Guid SellingPriceId { get; set; }
    public decimal Discount { get; set; }
    public DiscountType DiscountType { get; set; }
}


