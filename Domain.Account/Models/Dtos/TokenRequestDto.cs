namespace Domain.Account.Models.Dtos;

public class TokenRequestDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}