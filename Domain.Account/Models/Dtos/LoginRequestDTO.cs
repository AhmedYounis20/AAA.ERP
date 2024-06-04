namespace AAA.ERP;

public class LoginRequestDTO
{
    [Required]
    public string? UserName { get; set; }
    [Required]
    public string? Password { get; set; }
}
