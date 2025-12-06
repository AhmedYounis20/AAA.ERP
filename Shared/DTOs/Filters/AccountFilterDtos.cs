using Shared.DTOs;

namespace Shared.DTOs.Filters;

/// <summary>
/// Filter DTO for Chart of Accounts
/// </summary>
public class ChartOfAccountFilterDto : TreeFilterDto
{
    /// <summary>
    /// Filter by account nature (Debit/Credit)
    /// </summary>
    public int? AccountNature { get; set; }
    
    /// <summary>
    /// Filter by account guide
    /// </summary>
    public Guid? AccountGuidId { get; set; }
    
    /// <summary>
    /// Filter by active status
    /// </summary>
    public bool? IsActiveAccount { get; set; }
    
    /// <summary>
    /// Filter by posted status
    /// </summary>
    public bool? IsPostedAccount { get; set; }
    
    /// <summary>
    /// Filter by stop dealing status
    /// </summary>
    public bool? IsStopDealing { get; set; }
    
    /// <summary>
    /// Filter by sub-ledger type
    /// </summary>
    public int? SubLeadgerType { get; set; }
}

/// <summary>
/// Filter DTO for Entries (Journal, Payment, Receipt, etc.)
/// </summary>
public class EntryFilterDto : BaseFilterDto
{
    /// <summary>
    /// Filter by entry type (Opening, Journal, Payment, Receipt, Combined)
    /// </summary>
    public int? EntryType { get; set; }
    
    /// <summary>
    /// Filter by branch
    /// </summary>
    public Guid? BranchId { get; set; }
    
    /// <summary>
    /// Filter by financial period
    /// </summary>
    public Guid? FinancialPeriodId { get; set; }
    
    /// <summary>
    /// Filter by currency
    /// </summary>
    public Guid? CurrencyId { get; set; }
    
    /// <summary>
    /// Filter by entry date from
    /// </summary>
    public DateTime? EntryDateFrom { get; set; }
    
    /// <summary>
    /// Filter by entry date to
    /// </summary>
    public DateTime? EntryDateTo { get; set; }
    
    /// <summary>
    /// Search by entry number
    /// </summary>
    public string? EntryNumber { get; set; }
    
    /// <summary>
    /// Search by document number
    /// </summary>
    public string? DocumentNumber { get; set; }
}

/// <summary>
/// Filter DTO for Financial Periods
/// </summary>
public class FinancialPeriodFilterDto : SettingFilterDto
{
    /// <summary>
    /// Filter by period type
    /// </summary>
    public int? PeriodType { get; set; }
    
    /// <summary>
    /// Filter by year number
    /// </summary>
    public string? YearNumber { get; set; }
    
    /// <summary>
    /// Filter for periods that contain this date
    /// </summary>
    public DateTime? ContainsDate { get; set; }
}

/// <summary>
/// Filter DTO for Cost Centers
/// </summary>
public class CostCenterFilterDto : TreeFilterDto
{
    /// <summary>
    /// Filter by cost center type
    /// </summary>
    public int? CostCenterType { get; set; }
}

/// <summary>
/// Filter DTO for SubLeadgers (Customers, Suppliers, Banks, etc.)
/// </summary>
public class SubLeadgerFilterDto : TreeFilterDto
{
    /// <summary>
    /// Search by code
    /// </summary>
    public string? Code { get; set; }
    
    /// <summary>
    /// Search by phone number
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// Filter by chart of account
    /// </summary>
    public Guid? ChartOfAccountId { get; set; }
}

/// <summary>
/// Filter DTO for Currencies
/// </summary>
public class CurrencyFilterDto : SettingFilterDto
{
    /// <summary>
    /// Filter by symbol
    /// </summary>
    public string? Symbol { get; set; }
    
    /// <summary>
    /// Filter for default currency only
    /// </summary>
    public bool? IsDefault { get; set; }
}

