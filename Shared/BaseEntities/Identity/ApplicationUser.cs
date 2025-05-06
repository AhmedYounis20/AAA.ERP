using Microsoft.AspNetCore.Identity;

namespace Shared.BaseEntities.Identity;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }

    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}
