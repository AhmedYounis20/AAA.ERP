namespace ERP.BuildingBlocks.Domain.Entities.Attachments;

/// <summary>Stable content path. Storage changes stay behind this URL.</summary>
public static class AttachmentUrls
{
    public static string Content(Guid id) => $"/api/attachments/{id}/content";
}
