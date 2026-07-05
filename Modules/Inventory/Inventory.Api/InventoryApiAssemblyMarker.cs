using System.Reflection;

namespace Inventory.Api;

/// <summary>Assembly marker for controller discovery from <c>ERP.Host</c>.</summary>
public static class InventoryApiAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(InventoryApiAssemblyMarker).Assembly;
}
