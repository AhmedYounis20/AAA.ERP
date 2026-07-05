namespace ERP.BuildingBlocks.Application.Logging;

public static class ModuleNameResolver
{
    public static string? FromType(Type type)
    {
        var ns = type.Namespace ?? string.Empty;

        if (ns.StartsWith("Accounting.", StringComparison.Ordinal))
            return "Accounting";

        if (ns.StartsWith("Inventory.", StringComparison.Ordinal))
            return "Inventory";

        if (ns.StartsWith("Identity.", StringComparison.Ordinal))
            return "Identity";

        if (ns.StartsWith("Platform.", StringComparison.Ordinal))
            return "Platform";

        if (ns.StartsWith("Domain.Account.", StringComparison.Ordinal))
            return "Accounting";

        if (ns.Contains(".Controllers.Inventory", StringComparison.Ordinal))
            return "Inventory";

        if (ns.Contains(".Controllers.Account", StringComparison.Ordinal))
            return "Accounting";

        if (ns.Contains(".Controllers.Identity", StringComparison.Ordinal))
            return "Identity";

        if (ns.Contains(".Controllers.Audit", StringComparison.Ordinal))
            return "Platform";

        if (ns.StartsWith("ERP.", StringComparison.Ordinal))
            return "ERP";

        return null;
    }
}
