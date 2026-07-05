namespace Shared.DTOs;

/// <summary>Attachment payload shared across modules (wire-compatible with legacy Accounting DTO).</summary>
public class AttachmentDto
{
    public Guid? AttachmentId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string FileContent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
