using ERP.BuildingBlocks.Application.Logging.Enrichers;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.BuildingBlocks.Application.Extensions;

public static class LoggingServiceCollectionExtensions
{
    public static IServiceCollection AddErpStructuredLogging(this IServiceCollection services)
    {
        services.AddSingleton<CorrelationIdEnricher>();
        services.AddSingleton<ModuleEnricher>();
        services.AddSingleton<CommandNameEnricher>();
        return services;
    }
}
