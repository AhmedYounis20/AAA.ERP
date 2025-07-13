using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.InventoryTransactions;

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