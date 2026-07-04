using Microsoft.Extensions.DependencyInjection;

namespace Platform.Localization;

public static class PlatformLocalizationModuleExtensions
{
    public static IServiceCollection AddPlatformLocalization(this IServiceCollection services)
    {
        // Resources are embedded in Platform.Localization; host calls AddLocalization() separately.
        return services;
    }
}
