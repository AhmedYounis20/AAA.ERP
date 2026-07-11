using Inventory.Domain.Models.Entities.Variants;
using Inventory.Domain.Models.Entities.PackingUnits;
using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities;

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