using Domain.Account.Models.Dtos.Attachments;
using Domain.Account.Models.Entities.Attachments;

namespace Domain.Account.Utility;

public static class AttachmentsExtensions
{
    public static IEnumerable<Attachment> ToAttachment(this IEnumerable<AttachmentDto> dtos)
    {
        foreach (var dto in dtos)
        {
            Attachment newItem = new Attachment
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
                ContentType = attachment.FileContentType,
                CreatedAt = attachment.CreatedAt,
                ModifiedAt = attachment.ModifiedAt,
                AttachmentId = attachment.Id
            };
        }
    }
    
    public static void CopyTo(this AttachmentDto source, Attachment target)
    {
        target.FileData = source.ToArray();
        target.FileName = source.FileName;
        target.FileContentType = source.ContentType;
        target.ModifiedAt = DateTime.Now;
    }
    
    public static void CopyTo(this Attachment source, AttachmentDto target)
    {
        target.FileContent = Convert.ToBase64String(source.FileData);
        target.FileName = source.FileName;
        target.ContentType = source.FileContentType;
        target.ModifiedAt = source.ModifiedAt;
        target.CreatedAt = source.CreatedAt;
    }
    
}