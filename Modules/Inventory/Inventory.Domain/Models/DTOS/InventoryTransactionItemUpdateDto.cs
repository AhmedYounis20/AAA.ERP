namespace Inventory.Domain.Models.DTOS;

public class InventoryTransactionItemUpdateDto
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public Guid PackingUnitId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal TotalCost { get; set; }
    public string Notes { get; set; } = string.Empty;
}
