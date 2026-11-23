namespace ERP.BuildingBlocks.Application.Attachments;

public sealed class AttachmentContent
{
    public required byte[] Data { get; init; }
    public required string ContentType { get; init; }
    public required string FileName { get; init; }
}
