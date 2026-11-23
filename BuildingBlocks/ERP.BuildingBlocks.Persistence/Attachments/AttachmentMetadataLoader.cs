using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Domain.Entities.Attachments;
using ERP.BuildingBlocks.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ERP.BuildingBlocks.Persistence.Attachments;

/// <summary>Loads attachment name, type, and content URL without the file bytes.</summary>
public static class AttachmentMetadataLoader
{
    public static async Task<IReadOnlyDictionary<Guid, AttachmentDto>> LoadAsync(
        IApplicationDbContext context,
        IEnumerable<Guid> attachmentIds,
        CancellationToken cancellationToken = default)
    {
        var ids = attachmentIds.Where(id => id != Guid.Empty).Distinct().ToList();
        if (ids.Count == 0)
            return new Dictionary<Guid, AttachmentDto>();

        var rows = await context.Set<Attachment>()
            .AsNoTracking()
            .Where(attachment => ids.Contains(attachment.Id))
            .Select(attachment => new
            {
                attachment.Id,
                attachment.FileName,
                attachment.FileContentType
            })
            .ToListAsync(cancellationToken);

        return rows.ToDictionary(
            row => row.Id,
            row => AttachmentDto.FromMetadata(row.Id, row.FileName, row.FileContentType));
    }
}
