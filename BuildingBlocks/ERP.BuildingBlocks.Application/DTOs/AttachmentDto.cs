using System.Text.Json.Serialization;
using ERP.BuildingBlocks.Domain.Entities.Attachments;

namespace ERP.BuildingBlocks.Application.DTOs;

/// <summary>Attachment payload shared across modules. Reads carry <see cref="ContentUrl"/>; writes carry <see cref="FileContent"/> only when the file changed.</summary>
public class AttachmentDto
{
    public Guid? AttachmentId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;

    /// <summary>Base64 bytes on create or replace. Empty on an edit that did not change the file.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FileContent { get; set; }

    /// <summary>Relative URL of <c>GET /api/attachments/{id}/content</c>. Present on reads.</summary>
    public string? ContentUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public int Length => FileContent?.Length ?? 0;

    public bool HasFileContent => !string.IsNullOrWhiteSpace(FileContent);

    public override string ToString() => FileContent ?? string.Empty;

    public byte[] ToArray() =>
        string.IsNullOrEmpty(FileContent) ? [] : Convert.FromBase64String(FileContent);

    public static AttachmentDto FromMetadata(Guid id, string? fileName, string? contentType) => new()
    {
        AttachmentId = id,
        FileName = fileName ?? string.Empty,
        ContentType = contentType ?? string.Empty,
        ContentUrl = AttachmentUrls.Content(id)
    };
}
