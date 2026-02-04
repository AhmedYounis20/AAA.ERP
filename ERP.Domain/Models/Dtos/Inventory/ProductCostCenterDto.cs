namespace ERP.Domain.Models.Dtos.Inventory;

public class ProductCostCenterDto
{
    public Guid CostCenterId { get; set; }
    public decimal Percent { get; set; }
}
