using ERP.BuildingBlocks.Application.Attachments;
using ERP.BuildingBlocks.Domain.Entities.Attachments;
using ERP.BuildingBlocks.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Platform.Infrastructure.Services;

/// <summary>Stores attachment bytes in platform.Attachments. A later S3 store replaces this class only.</summary>
public class DatabaseAttachmentStore(IApplicationDbContext context) : IAttachmentStore
{
    public string ContentUrl(Guid id) => AttachmentUrls.Content(id);

    public async Task<Guid> SaveAsync(string? fileName, string? contentType, byte[] data, CancellationToken cancellationToken = default)
    {
        var attachment = new Attachment
        {
            Id = Guid.NewGuid(),
            FileName = fileName,
            FileContentType = contentType,
            FileData = data,
            StorageProvider = AttachmentStorageProvider.Database
        };
        attachment.StorageKey = attachment.Id.ToString();
        context.Set<Attachment>().Add(attachment);
        await context.SaveChangesAsync(cancellationToken);
        return attachment.Id;
    }

    public async Task ReplaceAsync(Guid id, string? fileName, string? contentType, byte[] data, CancellationToken cancellationToken = default)
    {
        var attachment = await context.Set<Attachment>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
            ?? throw new InvalidOperationException($"Attachment {id} was not found.");

        attachment.FileName = fileName;
        attachment.FileContentType = contentType;
        attachment.FileData = data;
        attachment.StorageProvider = AttachmentStorageProvider.Database;
        attachment.StorageKey = id.ToString();
        context.Set<Attachment>().Update(attachment);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<AttachmentContent?> OpenReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var row = await context.Set<Attachment>()
            .AsNoTracking()
            .Where(attachment => attachment.Id == id)
            .Select(attachment => new
            {
                attachment.FileData,
                attachment.FileContentType,
                attachment.FileName,
                attachment.StorageProvider
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (row is null || row.StorageProvider != AttachmentStorageProvider.Database || row.FileData is null || row.FileData.Length == 0)
            return null;

        return new AttachmentContent
        {
            Data = row.FileData,
            ContentType = string.IsNullOrWhiteSpace(row.FileContentType) ? "application/octet-stream" : row.FileContentType,
            FileName = string.IsNullOrWhiteSpace(row.FileName) ? id.ToString() : row.FileName
        };
    }

    public async Task DeleteAsync(IReadOnlyCollection<Guid> attachmentIds, CancellationToken cancellationToken = default)
    {
        if (attachmentIds.Count == 0)
            return;

        await context.Set<Attachment>()
            .Where(attachment => attachmentIds.Contains(attachment.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }
}
