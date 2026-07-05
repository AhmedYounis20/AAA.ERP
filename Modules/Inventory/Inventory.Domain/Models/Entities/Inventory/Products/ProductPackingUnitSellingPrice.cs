using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Products;

public class ProductPackingUnitSellingPrice : BaseEntity
{
    public Guid ProductPackingUnitId { get; set; }
    public ProductPackingUnit? ProductPackingUnit { get; set; }

    public Guid SellingPriceId { get; set; }
    public decimal Amount { get; set; }
}


