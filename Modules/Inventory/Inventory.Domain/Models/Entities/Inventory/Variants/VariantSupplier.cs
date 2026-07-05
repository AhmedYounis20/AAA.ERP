using Accounting.Domain.Models.Entities.SubLeadgers;
using Shared.BaseEntities;

namespace Inventory.Domain.Models.Entities.Variants;

public class VariantSupplier : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid SupplierId { get; set; }
}