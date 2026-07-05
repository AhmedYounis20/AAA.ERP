namespace Platform.Caching.Application;

/// <summary>
/// Application port for keyed in-memory caching used by lookup and query facades.
/// </summary>
public interface ICacheService
{
    /// <summary>Gets a cached reference-type value by key, or <c>null</c> on miss or error.</summary>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;

    /// <summary>Stores a value under <paramref name="key"/>; uses <see cref="CacheDurations.Medium"/> when <paramref name="expiration"/> is omitted.</summary>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class;

    /// <summary>Returns a cached value or invokes <paramref name="factory"/> once (stampede-safe) and caches the result.</summary>
    Task<T?> GetOrCreateAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>Removes a single cache entry by key.</summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>Removes all tracked entries whose keys start with <paramref name="prefix"/> (case-insensitive).</summary>
    Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default);

    /// <summary>Indicates whether <paramref name="key"/> is currently present in the cache.</summary>
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>Re-validates a cached entry for <paramref name="key"/> (implementation may extend or no-op).</summary>
    Task RefreshAsync(string key, CancellationToken cancellationToken = default);
}

/// <summary>
/// Canonical cache key segments and helpers for entity-scoped lookup and detail keys.
/// </summary>
public static class CacheKeys
{
    /// <summary>Prefix segment for currency lookup caches.</summary>
    public const string Currencies = "currencies";

    /// <summary>Prefix segment for branch lookup caches.</summary>
    public const string Branches = "branches";

    /// <summary>Prefix segment for chart-of-account lookup caches.</summary>
    public const string ChartOfAccounts = "chartofaccounts";

    /// <summary>Prefix segment for account-guide lookup caches.</summary>
    public const string AccountGuides = "accountguides";

    /// <summary>Prefix segment for financial-period lookup caches.</summary>
    public const string FinancialPeriods = "financialperiods";

    /// <summary>Prefix segment for cost-center lookup caches.</summary>
    public const string CostCenters = "costcenters";

    /// <summary>Prefix segment for packing-unit lookup caches.</summary>
    public const string PackingUnits = "packingunits";

    /// <summary>Prefix segment for selling-price lookup caches.</summary>
    public const string SellingPrices = "sellingprices";

    /// <summary>Prefix segment for color lookup caches.</summary>
    public const string Colors = "colors";

    /// <summary>Prefix segment for size lookup caches.</summary>
    public const string Sizes = "sizes";

    /// <summary>Prefix segment for GL settings lookup caches.</summary>
    public const string GLSettings = "glsettings";

    /// <summary>Suffix appended to an entity prefix for lookup lists (e.g. <c>currencies:lookups</c>).</summary>
    public const string LookupsSuffix = ":lookups";

    /// <summary>Suffix appended to an entity prefix for full lists (e.g. <c>currencies:all</c>).</summary>
    public const string AllSuffix = ":all";

    /// <summary>Builds a lookup-list cache key for <paramref name="entityType"/>.</summary>
    public static string GetLookupsKey(string entityType) => $"{entityType}{LookupsSuffix}";

    /// <summary>Builds an all-items cache key for <paramref name="entityType"/>.</summary>
    public static string GetAllKey(string entityType) => $"{entityType}{AllSuffix}";

    /// <summary>Builds a single-entity cache key for <paramref name="entityType"/> and <paramref name="id"/>.</summary>
    public static string GetEntityKey(string entityType, Guid id) => $"{entityType}:{id}";
}

/// <summary>
/// Default absolute expiration durations for cached entries.
/// </summary>
public static class CacheDurations
{
    /// <summary>Five minutes — short-lived or frequently invalidated data.</summary>
    public static readonly TimeSpan Short = TimeSpan.FromMinutes(5);

    /// <summary>Thirty minutes — default for lookup caches when expiration is not specified.</summary>
    public static readonly TimeSpan Medium = TimeSpan.FromMinutes(30);

    /// <summary>One hour — moderately stable reference data.</summary>
    public static readonly TimeSpan Long = TimeSpan.FromHours(1);

    /// <summary>Twenty-four hours — rarely changing configuration or master data.</summary>
    public static readonly TimeSpan VeryLong = TimeSpan.FromHours(24);
}
