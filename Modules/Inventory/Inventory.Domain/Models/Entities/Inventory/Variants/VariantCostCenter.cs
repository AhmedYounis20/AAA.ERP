using Accounting.Domain.Models.Entities.CostCenters;
using Shared.BaseEntities;

namespace Inventory.Domain.Models.Entities.Variants;

public class VariantCostCenter : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid CostCenterId { get; set; }
    public CostCenter? CostCenter { get; set; }
    public decimal Percent { get; set; }
}
