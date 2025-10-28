using Shared.Domain.BaseEntities;

namespace Inventory.Domain.Models.Entities.Items;

public class ItemSupplier : BaseEntity
{
    public Guid ItemId { get; set; }
    public Item? Item { get; set; }
    public Guid SupplierId { get; set; }
    // Navigation property removed - will be handled through shared commands
}
