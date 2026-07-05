using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Domain.Entities.Attachments;

public class Attachment : BaseEntity
{
    public byte[] FileData { get; set; }
    public string? FileName { get; set; }
    public string? FileContentType { get; set; }
}