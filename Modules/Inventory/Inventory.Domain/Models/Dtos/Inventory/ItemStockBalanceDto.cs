namespace ERP.Domain.Models.Dtos.Inventory;

public class ItemStockBalanceDto
{
    public Guid BranchId { get; set; }
    public Guid PackingUnitId { get; set; }
    public decimal CurrentBalance { get; set; }
} 