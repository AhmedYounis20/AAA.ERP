namespace Shared.Exceptions;

public class InternalServerException : Exception
{
    public string? Details { get; }
    public InternalServerException(string message) : base(message)
    {
    }
    public InternalServerException(string name, string details) : base(name)
    {
        Details = details;
    }
}