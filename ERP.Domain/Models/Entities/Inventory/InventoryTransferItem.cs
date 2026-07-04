using ERP.Domain.Models.Entities.Inventory.Variants;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory;

public class InventoryTransferItem : BaseEntity
{
    public Guid InventoryTransferId { get; set; }
    public InventoryTransfer? InventoryTransfer { get; set; }
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid PackingUnitId { get; set; }
    public PackingUnit? PackingUnit { get; set; }
    public decimal Quantity { get; set; }
} 