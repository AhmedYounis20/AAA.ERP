using Serilog.Core;
using Serilog.Events;

namespace ERP.BuildingBlocks.Application.Logging.Enrichers;

public sealed class CommandNameEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var commandName = RequestLoggingScope.CommandName;
        if (string.IsNullOrWhiteSpace(commandName))
            return;

        logEvent.AddPropertyIfAbsent(
            propertyFactory.CreateProperty(LoggingContextPropertyNames.CommandName, commandName));
    }
}
