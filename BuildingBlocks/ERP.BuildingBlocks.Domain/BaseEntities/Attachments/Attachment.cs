using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP.BuildingBlocks.Domain.Entities.Attachments;

public class Attachment : BaseEntity
{
    [JsonIgnore]
    public byte[]? FileData { get; set; }

    public string? FileName { get; set; }

    public string? FileContentType { get; set; }

    [JsonIgnore]
    public AttachmentStorageProvider StorageProvider { get; set; } = AttachmentStorageProvider.Database;

    [JsonIgnore]
    public string? StorageKey { get; set; }

    [NotMapped]
    [JsonPropertyName("attachmentId")]
    public Guid AttachmentId => Id;

    [NotMapped]
    [JsonPropertyName("contentUrl")]
    public string ContentUrl => Id == Guid.Empty ? string.Empty : AttachmentUrls.Content(Id);
}
