namespace ERP.Application.Services.Caching;

/// <summary>
/// Service for caching data with various operations
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Gets a cached item by key
    /// </summary>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Sets a cached item with optional expiration
    /// </summary>
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Gets an item from cache or creates it using the factory if not found
    /// </summary>
    Task<T?> GetOrCreateAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Removes a cached item by key
    /// </summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes all cached items matching a pattern
    /// </summary>
    Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a key exists in the cache
    /// </summary>
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refreshes the expiration time of a cached item
    /// </summary>
    Task RefreshAsync(string key, CancellationToken cancellationToken = default);
}

/// <summary>
/// Static class containing cache key constants and helpers
/// </summary>
public static class CacheKeys
{
    // Prefix for different entity types
    public const string Currencies = "currencies";
    public const string Branches = "branches";
    public const string ChartOfAccounts = "chartofaccounts";
    public const string AccountGuides = "accountguides";
    public const string FinancialPeriods = "financialperiods";
    public const string CostCenters = "costcenters";
    public const string PackingUnits = "packingunits";
    public const string SellingPrices = "sellingprices";
    public const string Colors = "colors";
    public const string Sizes = "sizes";
    public const string GLSettings = "glsettings";

    // Lookup suffix
    public const string LookupsSuffix = ":lookups";
    public const string AllSuffix = ":all";

    /// <summary>
    /// Generates a cache key for lookups
    /// </summary>
    public static string GetLookupsKey(string entityType) => $"{entityType}{LookupsSuffix}";

    /// <summary>
    /// Generates a cache key for all items
    /// </summary>
    public static string GetAllKey(string entityType) => $"{entityType}{AllSuffix}";

    /// <summary>
    /// Generates a cache key for a specific entity
    /// </summary>
    public static string GetEntityKey(string entityType, Guid id) => $"{entityType}:{id}";
}

/// <summary>
/// Cache expiration durations
/// </summary>
public static class CacheDurations
{
    public static readonly TimeSpan Short = TimeSpan.FromMinutes(5);
    public static readonly TimeSpan Medium = TimeSpan.FromMinutes(30);
    public static readonly TimeSpan Long = TimeSpan.FromHours(1);
    public static readonly TimeSpan VeryLong = TimeSpan.FromHours(24);
}

