namespace Identity.Application.Models.Dtos;

/// <summary>New token pair issued after a successful refresh.</summary>
public class RefreshTokenResponse
{
    /// <summary>Replacement JWT access token.</summary>
    public string AccessToken { get; set; }

    /// <summary>Replacement refresh token.</summary>
    public string RefreshToken { get; set; }
}
