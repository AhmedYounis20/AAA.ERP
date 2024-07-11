using Domain.Account.Models.Entities.Attachments;

namespace Domain.Account.Models.Entities.SubLeadgers;

public class Branch : SubLeadgerBaseEntity<Branch>
{
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public Guid? AttachmentId { get; set; }
    public virtual Attachment? Attachment { get; set; }
}