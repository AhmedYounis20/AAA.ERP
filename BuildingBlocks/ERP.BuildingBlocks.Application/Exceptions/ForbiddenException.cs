namespace Shared.Exceptions;

/// <summary>Thrown when the caller is authenticated but lacks permission for the requested resource.</summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string messageKey) : base(messageKey)
    {
    }
}
