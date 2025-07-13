using ERP.Domain.Models.Entities.Account.SubLeadgers;
using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Sizes;

public class StockBalance : BaseEntity
{
    public Guid ItemId { get; set; }
    public Item? Item { get; set; }
    public Guid PackingUnitId { get; set; }
    public PackingUnit? PackingUnit { get; set; }
    public Guid BranchId { get; set; }
    public Branch? Branch { get; set; }

    public decimal CurrentBalance { get; set; }
    public decimal MinimumBalance { get; set; }
    public decimal MaximumBalance { get; set; }

    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
}