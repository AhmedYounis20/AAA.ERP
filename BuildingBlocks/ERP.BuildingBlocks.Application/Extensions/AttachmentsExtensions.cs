using ERP.BuildingBlocks.Domain.Entities.Attachments;
using ERP.BuildingBlocks.Application.DTOs;

namespace ERP.BuildingBlocks.Application.Extensions;

/// <summary>Maps between <see cref="AttachmentDto"/> wire payloads and persistence <see cref="Attachment"/> entities.</summary>
public static class AttachmentsExtensions
{
    public static IEnumerable<Attachment> ToAttachment(this IEnumerable<AttachmentDto> dtos)
    {
        foreach (var dto in dtos)
        {
            if (!dto.HasFileContent)
                continue;

            var newItem = new Attachment
            {
                FileData = dto.ToArray(),
                FileContentType = dto.ContentType,
                CreatedAt = dto.CreatedAt,
                ModifiedAt = dto.ModifiedAt,
                FileName = dto.FileName,
                StorageProvider = AttachmentStorageProvider.Database
            };

            if (dto.AttachmentId.HasValue)
                newItem.Id = dto.AttachmentId.Value;
            newItem.StorageKey = newItem.Id == Guid.Empty ? null : newItem.Id.ToString();
            yield return newItem;
        }
    }

    public static IEnumerable<AttachmentDto> ToAttachmentDto(this IEnumerable<Attachment> attachments)
    {
        foreach (var attachment in attachments)
        {
            yield return AttachmentDto.FromMetadata(
                attachment.Id,
                attachment.FileName,
                attachment.FileContentType);
        }
    }

    public static void CopyTo(this AttachmentDto source, Attachment target)
    {
        target.FileName = source.FileName;
        target.FileContentType = source.ContentType;
        target.ModifiedAt = DateTime.UtcNow;
        if (source.HasFileContent)
        {
            target.FileData = source.ToArray();
            target.StorageProvider = AttachmentStorageProvider.Database;
            target.StorageKey = target.Id.ToString();
        }
    }

    public static void CopyTo(this Attachment source, AttachmentDto target)
    {
        target.FileContent = null;
        target.FileName = source.FileName ?? string.Empty;
        target.ContentType = source.FileContentType ?? string.Empty;
        target.ContentUrl = AttachmentUrls.Content(source.Id);
        target.ModifiedAt = source.ModifiedAt;
        target.CreatedAt = source.CreatedAt;
        target.AttachmentId = source.Id;
    }
}
