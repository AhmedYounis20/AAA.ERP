using Inventory.Domain.Models.Entities.Items;
using Inventory.Domain.Models.Entities.PackingUnits;
using Shared.Domain.BaseEntities;

namespace Inventory.Domain.Models.Entities.InventoryTransactions;

public class InventoryTransactionItem : BaseEntity
{
    public Guid InventoryTransactionId { get; set; }
    public Guid ItemId { get; set; }
    public Item? Item { get; set; }

    public Guid PackingUnitId { get; set; }
    public PackingUnit? PackingUnit { get; set; }

    public decimal Quantity { get; set; }
    public decimal TotalCost { get; set; }
}
