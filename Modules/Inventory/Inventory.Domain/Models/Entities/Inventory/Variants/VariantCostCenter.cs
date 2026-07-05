using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Variants;

public class VariantCostCenter : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid CostCenterId { get; set; }
    public decimal Percent { get; set; }
}
