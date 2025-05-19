namespace Domain.Account.Models.Dtos;

public class LoginResponseDTO
{
    public string? Name { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
 