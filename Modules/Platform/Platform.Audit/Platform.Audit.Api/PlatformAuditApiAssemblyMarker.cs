using System.Reflection;

namespace Platform.Audit.Api;

/// <summary>Assembly marker for controller discovery from <c>ERP.Host</c>.</summary>
public static class PlatformAuditApiAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(PlatformAuditApiAssemblyMarker).Assembly;
}
