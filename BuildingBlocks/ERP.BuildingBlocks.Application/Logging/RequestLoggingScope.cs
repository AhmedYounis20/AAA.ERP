namespace ERP.BuildingBlocks.Application.Logging;

/// <summary>
/// Async request-scoped values consumed by Serilog enrichers during command handling.
/// </summary>
public static class RequestLoggingScope
{
    private static readonly AsyncLocal<string?> CommandNameScope = new();
    private static readonly AsyncLocal<string?> ModuleScope = new();

    public static string? CommandName
    {
        get => CommandNameScope.Value;
        set => CommandNameScope.Value = value;
    }

    public static string? Module
    {
        get => ModuleScope.Value;
        set => ModuleScope.Value = value;
    }
}
