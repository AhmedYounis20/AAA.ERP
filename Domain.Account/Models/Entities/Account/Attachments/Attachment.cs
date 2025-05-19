using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Account.Attachments;

public class Attachment : BaseEntity
{
    public byte[] FileData { get; set; }
    public string? FileName { get; set; }
    public string? FileContentType { get; set; }
}