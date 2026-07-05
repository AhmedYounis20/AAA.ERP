using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.Products;
using ERP.Shared.BaseEntities.Attachments;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Inventory.Variants;

public class VariantAttachment : BaseEntity
{
    public Guid VariantId { get; set; }
    public Variant? Variant { get; set; }
    public Guid AttachmentId { get; set; }
    public Attachment? Attachment { get; set; }
    public AttachmentType AttachmentType { get; set; }
    public bool IsPrimary { get; set; }
    public int SortOrder { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
}