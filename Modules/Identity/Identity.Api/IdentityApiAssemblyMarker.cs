using System.Reflection;

namespace Identity.Api;

/// <summary>
/// Assembly marker for controller discovery from <c>ERP.Host</c> via <c>AddApplicationPart</c>.
/// Controllers will be added to this project during Phase 2 of the module API migration.
/// </summary>
public static class IdentityApiAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(IdentityApiAssemblyMarker).Assembly;
}
