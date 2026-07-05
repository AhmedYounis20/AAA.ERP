using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Serilog.Context;

namespace ERP.BuildingBlocks.Application.Logging.Middleware;

public sealed class RequestModuleMiddleware
{
    private readonly RequestDelegate _next;

    public RequestModuleMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var module = ResolveModule(context);
        RequestLoggingScope.Module = module;

        if (string.IsNullOrWhiteSpace(module))
        {
            await _next(context);
            return;
        }

        using (LogContext.PushProperty(LoggingContextPropertyNames.Module, module))
        {
            await _next(context);
        }
    }

    private static string? ResolveModule(HttpContext context)
    {
        var controllerAction = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
        return controllerAction is null
            ? null
            : ModuleNameResolver.FromType(controllerAction.ControllerTypeInfo.AsType());
    }
}
