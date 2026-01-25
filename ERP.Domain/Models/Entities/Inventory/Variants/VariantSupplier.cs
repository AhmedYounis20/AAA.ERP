using ERP.Domain.Models.Entities.Account.SubLeadgers;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Variants;

public class VariantSupplier : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid SupplierId { get; set; }
}