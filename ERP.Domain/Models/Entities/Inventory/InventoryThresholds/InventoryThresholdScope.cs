namespace ERP.Domain.Models.Entities.Inventory.InventoryThresholds;

/// <summary>
/// Scope of an inventory threshold: All branches or a specific branch.
/// </summary>
public enum InventoryThresholdScope
{
    All = 0,
    Branch = 1
}
