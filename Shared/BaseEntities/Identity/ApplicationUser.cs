namespace Domain.Account.Models.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
}
