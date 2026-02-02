using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Variants;

public class VariantPurchaseTax : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid TaxId { get; set; }
}
