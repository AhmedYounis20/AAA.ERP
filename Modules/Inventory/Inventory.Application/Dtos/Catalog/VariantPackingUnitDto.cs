using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Application.Dtos;

public class VariantPackingUnitDto
{
    public Guid PackingUnitId { get; set; }
    public string? Name { get; set; } = string.Empty;
    public int OrderNumber { get; set; }
    public decimal PartsCount { get; set; }
    public bool IsDefaultPackingUnit { get; set; }
    public bool IsDefaultSales { get; set; }
    public bool IsDefaultPurchases { get; set; }
    public decimal LastCostPrice { get; set; }
    public decimal AverageCostPrice { get; set; }
    public List<VariantPackingUnitSellingPriceDto> SellingPrices { get; set; } = [];
}











