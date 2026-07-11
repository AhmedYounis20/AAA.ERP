namespace ERP.BuildingBlocks.Persistence;

/// <summary>SQL Server schema names for module table separation (ADR-006 / 4.2.2).</summary>
public static class DbSchemaNames
{
    public const string Accounting = "accounting";
    public const string Inventory = "inventory";
    public const string Platform = "platform";
    public const string Identity = "identity";
}
