namespace Identity.Application.Models.Dtos;

public class TokenRequestDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
