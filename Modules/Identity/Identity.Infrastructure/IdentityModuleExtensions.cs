using Identity.Infrastructure.Services;
using Identity.Application;
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
