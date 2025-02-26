using Domain.Account.Utility;

namespace Domain.Account.Models.Dtos.Attachments;

public class AttachmentDto
{
    public Guid? AttachmentId { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public string FileContent { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public override string ToString()
    {
        return FileContent;
    }

    public int Length
    {
        get => FileContent.Length; 
    }

    public byte[] ToArray()
    {
        return FileMethods.ToBytes(FileContent);
    }
}