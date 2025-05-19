using ERP.Domain.Models.Entities.Account.Attachments;

namespace ERP.Domain.Models.Entities.Account.SubLeadgers;

public class Branch : SubLeadgerBaseEntity<Branch>
{
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public Guid? AttachmentId { get; set; }
    public virtual Attachment? Attachment { get; set; }
}