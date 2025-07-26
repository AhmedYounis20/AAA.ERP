using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory;

public class InventoryTransferItem : BaseEntity
{
    public Guid InventoryTransferId { get; set; }
    public InventoryTransfer? InventoryTransfer { get; set; }
    public Guid ItemId { get; set; }
    public Item? Item { get; set; }
    public Guid PackingUnitId { get; set; }
    public PackingUnit? PackingUnit { get; set; }
    public decimal Quantity { get; set; }
} 