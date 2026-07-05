using Inventory.Domain.Models.Entities.InventoryThresholds;

namespace Inventory.Domain.Models.Dtos;

public class InventoryThresholdDto
{
    /// <summary>Display order / level (1-based) for frontend.</summary>
    public int Level { get; set; }
    public decimal MinQuantity { get; set; }
    public decimal MaxQuantity { get; set; }
    public InventoryThresholdScope Scope { get; set; }
    public Guid? BranchId { get; set; }
    public bool EnableNotification { get; set; }
}
