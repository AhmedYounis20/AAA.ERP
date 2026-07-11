using ERP.BuildingBlocks.Application.DTOs;

namespace Accounting.Application.Dtos.Lookups;

public class BankLookupDto : LookupDto
{
    public string? Code { get; set; }
    public Guid? ChartOfAccountId { get; set; }
    public bool IsActive { get; set; }
}

