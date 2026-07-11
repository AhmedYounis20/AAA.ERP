using Inventory.Domain.Models.Entities.Variants;
using Inventory.Domain.Models.Entities.PackingUnits;
using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.InventoryTransactions;

public class InventoryTransactionItem : BaseEntity
{
    public Guid InventoryTransactionId { get; set; }
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }

    public Guid PackingUnitId { get; set; }
    public PackingUnit? PackingUnit { get; set; }

    public decimal Quantity { get; set; }
    public decimal TotalCost { get; set; }
}