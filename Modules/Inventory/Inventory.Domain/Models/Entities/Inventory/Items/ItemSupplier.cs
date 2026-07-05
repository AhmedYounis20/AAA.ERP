using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Items;

public class ItemSupplier : BaseEntity
{
    public Guid ItemId { get; set; }
    public Item? Item { get; set; }
    public Guid SupplierId { get; set; }
}
