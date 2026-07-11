using ERP.BuildingBlocks.Application.DTOs;

namespace Accounting.Application.Dtos.Lookups;

public class FinancialPeriodLookupDto : LookupDto
{
    public string? YearNumber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsClosed { get; set; }
}

