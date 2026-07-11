using Inventory.Domain.Models.Entities.Products;
using ERP.BuildingBlocks.Domain.Entities.Attachments;
using ERP.BuildingBlocks.Domain.Entities;

namespace Inventory.Domain.Models.Entities.Variants;

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