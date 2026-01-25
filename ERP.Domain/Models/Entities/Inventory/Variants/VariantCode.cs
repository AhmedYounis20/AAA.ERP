using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Variants;

public class VariantCode : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public VariantCodeType CodeType { get; set; }
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
}