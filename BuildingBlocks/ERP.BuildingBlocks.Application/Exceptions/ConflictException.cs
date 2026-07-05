namespace ERP.BuildingBlocks.Application.Exceptions;

/// <summary>Thrown when a create or update would violate a uniqueness or state constraint.</summary>
public class ConflictException : Exception
{
    public ConflictException(string messageKey) : base(messageKey)
    {
    }
}
