using Shared.Domain.BaseEntities;

namespace Shared.Domain.Attachments;

public class Attachment : BaseEntity
{
    public byte[] FileData { get; set; } = [];
    public string? FileName { get; set; }
    public string? FileContentType { get; set; }
}
