using Shared.BaseEntities;
using Inventory.Domain.Models.Entities.Items;
using Domain.Account.Models.Dtos.Attachments;
using Inventory.Domain.Models.Entities.Products;

namespace Inventory.Domain.Models.Dtos;

public class ProductAttachmentDto
{
    public Guid? Id { get; set; }
    public AttachmentType AttachmentType { get; set; }
    public string? Description { get; set; }
    public bool IsPrimary { get; set; }
    public int DisplayOrder { get; set; }
    public AttachmentDto? Attachment { get; set; }
}
