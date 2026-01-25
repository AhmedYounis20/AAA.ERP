using Shared.BaseEntities;

namespace ERP.Shared.BaseEntities.Attachments;

public class Attachment : BaseEntity
{
    public byte[] FileData { get; set; }
    public string? FileName { get; set; }
    public string? FileContentType { get; set; }
}