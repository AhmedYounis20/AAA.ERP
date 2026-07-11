namespace Identity.Application.Models.Dtos;

/// <summary>Credentials submitted for user login.</summary>
public class LoginRequestDto
{
    /// <summary>Application username.</summary>
    [Required]
    public string? UserName { get; set; }

    /// <summary>User password.</summary>
    [Required]
    public string? Password { get; set; }
}
