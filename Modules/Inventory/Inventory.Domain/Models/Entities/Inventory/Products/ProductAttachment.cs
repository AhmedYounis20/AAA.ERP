using ERP.Shared.BaseEntities.Attachments;
using Shared.BaseEntities;

namespace Inventory.Domain.Models.Entities.Products;

public class ProductAttachment : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    public Guid AttachmentId { get; set; }
    public Attachment? Attachment { get; set; }
    public AttachmentType AttachmentType { get; set; }
    public bool IsPrimary { get; set; }
    public int SortOrder { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
}