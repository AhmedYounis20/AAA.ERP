using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Variants;

public class VariantSalesTax : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid TaxId { get; set; }
}
