using ERP.Domain.Models.Entities.Account.Attachments;
using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Account.Entries;

public class EntryAttachment : BaseEntity
{
    public Guid EntryId { get; set; }
    public Guid AttachmentId { get; set; }
    public Attachment Attachment { get; set; }
}