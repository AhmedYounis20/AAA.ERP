namespace Shared.Exceptions;

public class BadRequestException : Exception
{
    public string? Details { get; }
    public BadRequestException(string message) : base(message)
    {
    }
    public BadRequestException(string name, string details) : base(name)
    {
        Details = details;
    }
}