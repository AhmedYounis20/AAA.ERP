using ERP.Application.Services.Caching;
using Microsoft.Extensions.DependencyInjection;
using Platform.Caching.Infrastructure.Services;

namespace Platform.Caching.Infrastructure;

public static class PlatformCachingModuleExtensions
{
    public static IServiceCollection AddPlatformCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();
        return services;
    }
}
