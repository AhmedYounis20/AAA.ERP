using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Serilog.Core;
using Serilog.Events;

namespace ERP.BuildingBlocks.Application.Logging.Enrichers;

public sealed class ModuleEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ModuleEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var module = RequestLoggingScope.Module
            ?? ResolveFromEndpoint(_httpContextAccessor.HttpContext);

        if (string.IsNullOrWhiteSpace(module))
            return;

        logEvent.AddPropertyIfAbsent(
            propertyFactory.CreateProperty(LoggingContextPropertyNames.Module, module));
    }

    private static string? ResolveFromEndpoint(HttpContext? httpContext)
    {
        var controllerAction = httpContext?.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
        return controllerAction is null
            ? null
            : ModuleNameResolver.FromType(controllerAction.ControllerTypeInfo.AsType());
    }
}
