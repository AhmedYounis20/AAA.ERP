using Shared.BaseEntities;
using ERP.Domain.Models.Entities.Inventory.Items;
using Domain.Account.Models.Dtos.Attachments;
using ERP.Domain.Models.Entities.Inventory.Products;

namespace ERP.Domain.Models.Dtos.Inventory;

public class ProductAttachmentDto
{
    public Guid? Id { get; set; }
    public AttachmentType AttachmentType { get; set; }
    public string? Description { get; set; }
    public bool IsPrimary { get; set; }
    public int DisplayOrder { get; set; }
    public AttachmentDto? Attachment { get; set; }
}
