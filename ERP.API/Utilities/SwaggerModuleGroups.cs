namespace ERP.Api.Utilities;

public static class SwaggerModuleGroups
{
    public const string All = "v1";
    public const string Accounting = "accounting";
    public const string Inventory = "inventory";
    public const string Identity = "identity";
    public const string Platform = "platform";

    public static IReadOnlyList<(string DocumentName, string DisplayName)> Documents { get; } =
    [
        (All, "All APIs"),
        (Accounting, "Accounting"),
        (Inventory, "Inventory"),
        (Identity, "Identity"),
        (Platform, "Platform")
    ];
}
