using ERP.Domain.Models.Entities.Account.CostCenters;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Products;

public class ProductCostCenter : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid CostCenterId { get; set; }
    public CostCenter? CostCenter { get; set; }
    public decimal Percent { get; set; }
}
