using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Models;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }

    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; set; }
}
