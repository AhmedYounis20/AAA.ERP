using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace ERP.BuildingBlocks.Application.Logging.Middleware;

public sealed class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[LoggingContextPropertyNames.CorrelationIdHeader]
            .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(correlationId))
            correlationId = Guid.NewGuid().ToString("D");

        context.Items[LoggingContextPropertyNames.CorrelationIdItemKey] = correlationId;
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[LoggingContextPropertyNames.CorrelationIdHeader] = correlationId;
            return Task.CompletedTask;
        });

        try
        {
            using (LogContext.PushProperty(LoggingContextPropertyNames.CorrelationId, correlationId))
            {
                await _next(context);
            }
        }
        finally
        {
            RequestLoggingScope.CommandName = null;
            RequestLoggingScope.Module = null;
        }
    }
}
