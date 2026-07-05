using ERP.BuildingBlocks.Application.Logging.Enrichers;
using Serilog;

namespace ERP.Api.Utilities;

/// <summary>
/// Serilog host integration (Phase 4.1.1).
/// Uses <c>builder.Host.UseSerilog</c> so the generic host owns flush/dispose on shutdown.
/// Do not call explicit Serilog flush/dispose in Program — it breaks WebApplicationFactory host reuse.
/// </summary>
public static class SerilogExtensions
{
    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog(
            (context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.With(services.GetRequiredService<CorrelationIdEnricher>())
                .Enrich.With(services.GetRequiredService<ModuleEnricher>())
                .Enrich.With(services.GetRequiredService<CommandNameEnricher>()),
            preserveStaticLogger: true);

        return builder;
    }
}
