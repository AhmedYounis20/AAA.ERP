using Inventory.Domain.Models.Entities.Variants;
using Inventory.Domain.Models.Entities.PackingUnits;
using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Sizes;

public class StockBalance : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid PackingUnitId { get; set; }
    public PackingUnit? PackingUnit { get; set; }
    public Guid BranchId { get; set; }

    public decimal CurrentBalance { get; set; }
    public decimal MinimumBalance { get; set; }
    public decimal MaximumBalance { get; set; }

    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
}
