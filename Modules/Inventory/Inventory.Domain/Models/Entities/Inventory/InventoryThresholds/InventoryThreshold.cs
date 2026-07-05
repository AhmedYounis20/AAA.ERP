using Inventory.Domain.Models.Entities.Products;
using Inventory.Domain.Models.Entities.Variants;
using Shared.BaseEntities;

namespace Inventory.Domain.Models.Entities.InventoryThresholds;

/// <summary>
/// Inventory threshold for stock levels and alerts at product or variant level.
/// Defines quantity ranges and optional notification per scope (All | Branch).
/// BranchId references Accounting domain (Branch); no navigation to keep domains separate.
/// </summary>
public class InventoryThreshold : BaseEntity
{
    public OwnerType OwnerType { get; set; }
    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid? VariantId { get; set; }
    public Variant? Variant { get; set; }

    /// <summary>Display order / level (1-based) for frontend.</summary>
    public int Level { get; set; }
    public decimal MinQuantity { get; set; }
    public decimal MaxQuantity { get; set; }
    public InventoryThresholdScope Scope { get; set; }
    /// <summary>Branch (Accounting domain); stored as FK only, no navigation.</summary>
    public Guid? BranchId { get; set; }
    public bool EnableNotification { get; set; }
}
