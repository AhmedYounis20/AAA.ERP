using ERP.Domain.Models.Entities.Inventory.InventoryThresholds;
using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Domain.Models.Entities.Inventory.Variants;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.ExpiryLevels;

public class ExpiryLevel : BaseEntity
{
    public OwnerType OwnerType { get; set; }
    public Guid? ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid? VariantId { get; set; }
    public Variant? Variant { get; set; }

    /// <summary>Display order / level (1-based) for frontend.</summary>
    public int Level { get; set; }
    public int DaysFrom { get; set; }
    public int DaysTo { get; set; }
    public bool EnableNotification { get; set; }
}