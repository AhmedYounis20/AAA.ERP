using Microsoft.EntityFrameworkCore;

namespace Shared.DTOs;

/// <summary>
/// Represents a paginated result set with metadata for client-side pagination
/// </summary>
/// <typeparam name="T">The type of items in the result set</typeparam>
public class PaginatedResult<T>
{
    public List<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PaginatedResult() { }

    public PaginatedResult(List<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    /// <summary>
    /// Creates a PaginatedResult from an IQueryable with async execution
    /// </summary>
    public static async Task<PaginatedResult<T>> CreateAsync(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<T>(items, count, pageNumber, pageSize);
    }

    /// <summary>
    /// Maps the items to a different type
    /// </summary>
    public PaginatedResult<TDestination> Map<TDestination>(Func<T, TDestination> mapper)
    {
        return new PaginatedResult<TDestination>
        {
            Items = Items.Select(mapper).ToList(),
            TotalCount = TotalCount,
            PageNumber = PageNumber,
            PageSize = PageSize
        };
    }
}

/// <summary>
/// Extension methods for creating paginated results
/// </summary>
public static class PaginatedResultExtensions
{
    public static Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return PaginatedResult<T>.CreateAsync(source, pageNumber, pageSize, cancellationToken);
    }
}

