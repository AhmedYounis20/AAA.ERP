using ERP.Domain.Models.Entities.Inventory.SellingPrices;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Items;

public class ItemPackingUnitSellingPrice : BaseEntity
{
    public Guid ItemPackingUnitPriceId { get; set; }
    public Guid sellingPriceId { get; set; }
    public SellingPrice? SellingPrice { get; set; }
    public decimal Amount { get; set; }
}