using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace ERP.BuildingBlocks.Application.Logging.Enrichers;

public sealed class CorrelationIdEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var correlationId = _httpContextAccessor.HttpContext?.Items[LoggingContextPropertyNames.CorrelationIdItemKey] as string;
        if (string.IsNullOrWhiteSpace(correlationId))
            return;

        logEvent.AddPropertyIfAbsent(
            propertyFactory.CreateProperty(LoggingContextPropertyNames.CorrelationId, correlationId));
    }
}
