using Identity.Application.Services;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Servcies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.DataProtection;
using Identity.Domain.Models;
using Shared.Application.ModuleRegistration;

namespace Identity.Infrastructure.ModuleRegistration;

public class IdentityModuleRegistration : IModuleRegistration
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Register Services
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddHttpContextAccessor();

        // Data Protection required by Identity token providers
        services.AddDataProtection();

        // ASP.NET Core Identity
        services
            .AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

    }

    public void RegisterDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Identity.Infrastructure")));
    }

    public void RegisterMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(IdentityModuleRegistration).Assembly);
        });
    }

    public void RegisterControllers(IServiceCollection services)
    {
        // Controllers will be registered automatically by ASP.NET Core
        // when they are in the same assembly or referenced assemblies
    }
}
