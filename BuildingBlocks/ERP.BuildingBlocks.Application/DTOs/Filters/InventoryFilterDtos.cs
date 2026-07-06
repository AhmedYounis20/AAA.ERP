using ERP.BuildingBlocks.Application.DTOs;

namespace ERP.BuildingBlocks.Application.DTOs.Filters;

/// <summary>
/// Filter DTO for Inventory Transactions
/// </summary>
public class InventoryTransactionFilterDto : BaseFilterDto
{
    /// <summary>
    /// Filter by transaction type (Import, Export)
    /// </summary>
    public int? TransactionType { get; set; }

    /// <summary>
    /// Filter by branch
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Filter by variant
    /// </summary>
    public Guid? VariantId { get; set; }

    /// <summary>
    /// Filter by transaction date from
    /// </summary>
    public DateTime? TransactionDateFrom { get; set; }

    /// <summary>
    /// Filter by transaction date to
    /// </summary>
    public DateTime? TransactionDateTo { get; set; }

    /// <summary>
    /// Search by transaction number
    /// </summary>
    public string? TransactionNumber { get; set; }
}

/// <summary>
/// Filter DTO for Stock Balances
/// </summary>
public class StockBalanceFilterDto : BaseFilterDto
{
    /// <summary>
    /// Filter by branch
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Filter by variant
    /// </summary>
    public Guid? VariantId { get; set; }

    /// <summary>
    /// Filter by packing unit
    /// </summary>
    public Guid? PackingUnitId { get; set; }

    /// <summary>
    /// Filter for items with low stock (below minimum)
    /// </summary>
    public bool? LowStock { get; set; }

    /// <summary>
    /// Filter for items with zero stock
    /// </summary>
    public bool? ZeroStock { get; set; }
}

/// <summary>
/// Filter DTO for Packing Units
/// </summary>
public class PackingUnitFilterDto : SettingFilterDto
{
    /// <summary>
    /// Search by symbol
    /// </summary>
    public string? Symbol { get; set; }
}

/// <summary>
/// Filter DTO for Selling Prices
/// </summary>
public class SellingPriceFilterDto : SettingFilterDto
{
    /// <summary>
    /// Filter for default selling price only
    /// </summary>
    public bool? IsDefault { get; set; }
}

/// <summary>
/// Filter DTO for Colors
/// </summary>
public class ColorFilterDto : SettingFilterDto
{
    /// <summary>
    /// Search by color code
    /// </summary>
    public string? Code { get; set; }
}

/// <summary>
/// Filter DTO for Sizes
/// </summary>
public class SizeFilterDto : SettingFilterDto
{
    /// <summary>
    /// Search by size code
    /// </summary>
    public string? Code { get; set; }
}

/// <summary>
/// Filter DTO for Inventory Transfers
/// </summary>
public class InventoryTransferFilterDto : BaseFilterDto
{
    /// <summary>
    /// Filter by source branch
    /// </summary>
    public Guid? FromBranchId { get; set; }

    /// <summary>
    /// Filter by destination branch
    /// </summary>
    public Guid? ToBranchId { get; set; }

    /// <summary>
    /// Filter by transfer date from
    /// </summary>
    public DateTime? TransferDateFrom { get; set; }

    /// <summary>
    /// Filter by transfer date to
    /// </summary>
    public DateTime? TransferDateTo { get; set; }

    /// <summary>
    /// Filter by status
    /// </summary>
    public int? Status { get; set; }
}

