using Shared.BaseEntities;

namespace Domain.Account.Models.Entities.Attachments;

public class Attachment: BaseEntity
{
    public Byte[] FileData { get; set; }
    public string? FileName { get; set; }
    public string? FileContentType { get; set; }
}