using Inventory.Domain.Models.Entities.Items;
using Inventory.Domain.Models.Entities.PackingUnits;
using Shared.Domain.BaseEntities;

namespace Inventory.Domain.Models.Entities.StockBalances;

public class StockBalance : BaseEntity
{
    public Guid ItemId { get; set; }
    public Item? Item { get; set; }
    public Guid PackingUnitId { get; set; }
    public PackingUnit? PackingUnit { get; set; }
    public Guid BranchId { get; set; }
    // Navigation property removed - will be handled through shared commands

    public decimal CurrentBalance { get; set; }
    public decimal MinimumBalance { get; set; }
    public decimal MaximumBalance { get; set; }

    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
}
