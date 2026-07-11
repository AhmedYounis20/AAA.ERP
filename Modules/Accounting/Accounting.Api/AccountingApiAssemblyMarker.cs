using System.Reflection;

namespace Accounting.Api;

/// <summary>Assembly marker for controller discovery from <c>ERP.Host</c>.</summary>
public static class AccountingApiAssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AccountingApiAssemblyMarker).Assembly;
}
