using Domain.Account.Services.Identity;
using ERP.Application.Services.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class IdentityModuleExtensions
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICurrentUser, CurrentUserService>();
        return services;
    }
}
