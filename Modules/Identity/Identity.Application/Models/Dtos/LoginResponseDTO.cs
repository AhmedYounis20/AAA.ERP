namespace Identity.Application.Models.Dtos;

/// <summary>Successful login payload returned to API clients.</summary>
public class LoginResponseDTO
{
    /// <summary>User display name.</summary>
    public string? Name { get; set; }

    /// <summary>Application username.</summary>
    public string? UserName { get; set; }

    /// <summary>User email address.</summary>
    public string? Email { get; set; }

    /// <summary>JWT access token.</summary>
    public string? AccessToken { get; set; }

    /// <summary>Opaque refresh token for subsequent renewal.</summary>
    public string? RefreshToken { get; set; }
}
