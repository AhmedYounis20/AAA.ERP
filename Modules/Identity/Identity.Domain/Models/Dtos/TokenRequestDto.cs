namespace Identity.Domain.Models.Dtos;

public class TokenRequestDto
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
