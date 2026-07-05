namespace Identity.Contracts;

/// <summary>
/// Provides the authenticated user context for the current HTTP request.
/// </summary>
public interface ICurrentUser
{
    /// <summary>Current user identifier, or null when unauthenticated.</summary>
    Guid? UserId { get; }

    /// <summary>Current user name, or null when unauthenticated.</summary>
    string? UserName { get; }
}
