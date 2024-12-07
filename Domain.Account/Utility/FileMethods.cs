using System.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
namespace Domain.Account.Utility;

public static class FileMethods
{
    public static byte[] ToBytes(string base64)
    {
        return System.Convert.FromBase64String(base64);
    }

    public static string ToBase64(byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }

    static string? DetectImageFormat(byte[] fileContent)
    {
        using var stream = new MemoryStream(fileContent);
        IImageFormat format = Image.DetectFormat(stream);

        return format?.Name.ToLower() switch
        {
            "png" => ".png",
            "jpeg" => ".jpg",
            "gif" => ".gif",
            "bmp" => ".bmp",
            "tiff" => ".tiff",
            _ => null // Unknown format
        };
    }
}