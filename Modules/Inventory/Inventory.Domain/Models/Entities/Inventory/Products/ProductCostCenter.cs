using Accounting.Domain.Models.Entities.CostCenters;
using Shared.BaseEntities;

namespace Inventory.Domain.Models.Entities.Products;

public class ProductCostCenter : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid CostCenterId { get; set; }
    public CostCenter? CostCenter { get; set; }
    public decimal Percent { get; set; }
}
