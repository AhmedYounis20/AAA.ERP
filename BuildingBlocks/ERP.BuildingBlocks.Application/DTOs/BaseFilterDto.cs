namespace Shared.DTOs;

/// <summary>
/// Base filter DTO for paginated GET requests
/// </summary>
public class BaseFilterDto
{
    private int _pageNumber = 1;
    private int _pageSize = 10;
    private const int MaxPageSize = 100;

    /// <summary>
    /// Page number (1-based, defaults to 1)
    /// </summary>
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    /// <summary>
    /// Page size (defaults to 10, max 100)
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? 10 : value);
    }

    /// <summary>
    /// Search term for filtering by name or other text fields
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Property name to sort by
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sort direction (true for descending, false for ascending)
    /// </summary>
    public bool SortDescending { get; set; }

    /// <summary>
    /// Filter by creation date from
    /// </summary>
    public DateTime? CreatedFrom { get; set; }

    /// <summary>
    /// Filter by creation date to
    /// </summary>
    public DateTime? CreatedTo { get; set; }

    /// <summary>
    /// Include soft-deleted records (admin only)
    /// </summary>
    public bool IncludeDeleted { get; set; }
}

/// <summary>
/// Filter DTO for entities with Name fields (BaseSettingEntity)
/// </summary>
public class SettingFilterDto : BaseFilterDto
{
    /// <summary>
    /// Filter by active status (if entity supports it)
    /// </summary>
    public bool? IsActive { get; set; }
}

/// <summary>
/// Filter DTO for tree-structured entities
/// </summary>
public class TreeFilterDto : SettingFilterDto
{
    /// <summary>
    /// Filter by parent ID (null for root items)
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Include children in the response
    /// </summary>
    public bool IncludeChildren { get; set; }
}

