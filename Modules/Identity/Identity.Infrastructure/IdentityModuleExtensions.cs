using Identity.Infrastructure.Services;
using Identity.Application;
using ERP.Infrastructure.DBConfiguration.DbContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ERP.BuildingBlocks.Domain.Entities.Identity;

namespace Identity.Infrastructure;

public static class IdentityModuleExtensions
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICurrentUser, CurrentUserService>();
        return services;
    }
}
