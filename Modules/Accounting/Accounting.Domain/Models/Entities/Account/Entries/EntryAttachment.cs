using ERP.BuildingBlocks.Domain.Entities.Attachments;
using ERP.BuildingBlocks.Domain.Entities;

namespace Accounting.Domain.Models.Entities.Entries;

public class EntryAttachment : BaseEntity
{
    public Guid EntryId { get; set; }
    public Guid AttachmentId { get; set; }
    public Attachment Attachment { get; set; }
}