namespace Inventory.Domain.Models.Dtos.Inventory;

/// <summary>Stock balance read model with branch display name.</summary>
public class StockBalanceReadDto
{
    public Guid Id { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public Guid BranchId { get; set; }
    public string? BranchName { get; set; }
    public Guid VariantId { get; set; }
    public Guid PackingUnitId { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal MinimumBalance { get; set; }
    public decimal MaximumBalance { get; set; }
    public decimal UnitCost { get; set; }
    public decimal TotalCost { get; set; }
}
