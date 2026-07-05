namespace Identity.Application.Models.Dtos;

/// <summary>Payload for registering a new application user.</summary>
public class RegisterRequestDTO
{
    /// <summary>Email address used as the username.</summary>
    public string? Email { get; set; }

    /// <summary>Display name (optional).</summary>
    public string? Name { get; set; }

    /// <summary>Initial password.</summary>
    public string? Password { get; set; }

    /// <summary>Requested role (optional; defaults applied by the service).</summary>
    public string? Role { get; set; }
}
