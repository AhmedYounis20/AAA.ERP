namespace ERP.BuildingBlocks.Domain.Entities.Attachments;

/// <summary>Where the bytes for an attachment live. Business modules never branch on this.</summary>
public enum AttachmentStorageProvider
{
    Database = 0
}
