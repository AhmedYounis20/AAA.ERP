using Shared.BaseEntities;

namespace Inventory.Domain.Models.Entities.Variants;

public class VariantPurchaseTax : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid TaxId { get; set; }
}
