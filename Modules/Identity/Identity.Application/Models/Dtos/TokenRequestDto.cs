namespace Identity.Application.Models.Dtos;

/// <summary>Expired access token and matching refresh token for renewal.</summary>
public class TokenRequestDto
{
    /// <summary>Previously issued JWT access token (may be expired).</summary>
    public string AccessToken { get; set; }

    /// <summary>Refresh token issued alongside the access token.</summary>
    public string RefreshToken { get; set; }
}
