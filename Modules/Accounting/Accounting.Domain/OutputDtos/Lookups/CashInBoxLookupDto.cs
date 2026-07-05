using Shared.DTOs;

namespace ERP.Domain.OutputDtos.Lookups;

public class CashInBoxLookupDto : LookupDto
{
    public string? Code { get; set; }
    public Guid? ChartOfAccountId { get; set; }
    public Guid? BranchId { get; set; }
    public bool IsActive { get; set; }
}

