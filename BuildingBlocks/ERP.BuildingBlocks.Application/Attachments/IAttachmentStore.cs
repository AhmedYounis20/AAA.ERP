namespace ERP.BuildingBlocks.Application.Attachments;

/// <summary>
/// Project-level attachment storage. Business modules pass bytes in and ids out;
/// they do not choose a schema or a storage technology.
/// </summary>
public interface IAttachmentStore
{
    Task<Guid> SaveAsync(string? fileName, string? contentType, byte[] data, CancellationToken cancellationToken = default);

    Task ReplaceAsync(Guid id, string? fileName, string? contentType, byte[] data, CancellationToken cancellationToken = default);

    Task<AttachmentContent?> OpenReadAsync(Guid id, CancellationToken cancellationToken = default);

    Task DeleteAsync(IReadOnlyCollection<Guid> attachmentIds, CancellationToken cancellationToken = default);

    string ContentUrl(Guid id);
}
