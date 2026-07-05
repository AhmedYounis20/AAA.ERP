using ERP.BuildingBlocks.Application.Logging;
using MediatR;
using Serilog.Context;

namespace Shared.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var commandName = typeof(TRequest).Name;
        var module = ModuleNameResolver.FromType(typeof(TRequest)) ?? RequestLoggingScope.Module;

        RequestLoggingScope.CommandName = commandName;
        if (!string.IsNullOrWhiteSpace(module))
            RequestLoggingScope.Module = module;

        IDisposable? moduleScope = string.IsNullOrWhiteSpace(module)
            ? null
            : LogContext.PushProperty(LoggingContextPropertyNames.Module, module);

        using (LogContext.PushProperty(LoggingContextPropertyNames.CommandName, commandName))
        using (moduleScope)
        {
            return await next();
        }
    }
}
