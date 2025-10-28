namespace Account.Domain.Models.Entities.SubLeadgers;

public class Branch : SubLeadgerBaseEntity<Branch>
{
    public string? Code { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public Guid? AttachmentId { get; set; }
    public virtual Attachment? Attachment { get; set; }
}
