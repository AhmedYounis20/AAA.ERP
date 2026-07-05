namespace Inventory.Domain.Models.Dtos;

public class ProductCostCenterDto
{
    public Guid CostCenterId { get; set; }
    public decimal Percent { get; set; }
}
