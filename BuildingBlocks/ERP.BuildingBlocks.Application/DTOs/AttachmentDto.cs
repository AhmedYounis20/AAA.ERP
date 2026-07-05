namespace ERP.BuildingBlocks.Application.DTOs;

/// <summary>Attachment payload shared across modules (wire-compatible JSON shape).</summary>
public class AttachmentDto
{
    public Guid? AttachmentId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string FileContent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public int Length => FileContent?.Length ?? 0;

    public override string ToString() => FileContent ?? string.Empty;

    public byte[] ToArray() =>
        string.IsNullOrEmpty(FileContent) ? [] : Convert.FromBase64String(FileContent);
}
