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
            var newItem = new Attachment
            {
                FileData = dto.ToArray(),
                FileContentType = dto.ContentType,
                CreatedAt = dto.CreatedAt,
                ModifiedAt = dto.ModifiedAt,
                FileName = dto.FileName,
            };

            if (dto.AttachmentId.HasValue)
                newItem.Id = dto.AttachmentId.Value;
            yield return newItem;
        }
    }

    public static IEnumerable<AttachmentDto> ToAttachmentDto(this IEnumerable<Attachment> attachments)
    {
        foreach (var attachment in attachments)
        {
            yield return new AttachmentDto
            {
                FileContent = Convert.ToBase64String(attachment.FileData),
                ContentType = attachment.FileContentType ?? string.Empty,
                CreatedAt = attachment.CreatedAt,
                ModifiedAt = attachment.ModifiedAt,
                AttachmentId = attachment.Id,
                FileName = attachment.FileName ?? string.Empty
            };
        }
    }

    public static void CopyTo(this AttachmentDto source, Attachment target)
    {
        target.FileData = source.ToArray();
        target.FileName = source.FileName;
        target.FileContentType = source.ContentType;
        target.ModifiedAt = DateTime.UtcNow;
    }

    public static void CopyTo(this Attachment source, AttachmentDto target)
    {
        target.FileContent = Convert.ToBase64String(source.FileData);
        target.FileName = source.FileName ?? string.Empty;
        target.ContentType = source.FileContentType ?? string.Empty;
        target.ModifiedAt = source.ModifiedAt;
        target.CreatedAt = source.CreatedAt;
        target.AttachmentId = source.Id;
    }
}
