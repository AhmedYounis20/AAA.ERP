namespace ERP.Application.Services.Identity;

public interface ICurrentUser
{
    Guid? UserId { get; }
    string? UserName { get; }
}
