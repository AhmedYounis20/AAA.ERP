using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Products;

public class ProductPackingUnitSellingPrice : BaseEntity
{
    public Guid ProductPackingUnitId { get; set; }
    public ProductPackingUnit? ProductPackingUnit { get; set; }
    
    public Guid SellingPriceId { get; set; }
    public decimal Amount { get; set; }
}


