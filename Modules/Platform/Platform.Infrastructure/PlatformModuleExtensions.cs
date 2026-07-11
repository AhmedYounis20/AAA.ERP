using Microsoft.Extensions.DependencyInjection;
using Platform.Audit.Infrastructure;
using Platform.Caching.Infrastructure;
using Platform.Localization;

namespace Platform.Infrastructure;

public static class PlatformModuleExtensions
{
    public static IServiceCollection AddPlatformModules(this IServiceCollection services)
    {
        services.AddPlatformAudit();
        services.AddPlatformCaching();
        services.AddPlatformLocalization();
        return services;
    }
}
