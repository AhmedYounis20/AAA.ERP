using ERP.BuildingBlocks.Application.DTOs;

namespace Accounting.Domain.OutputDtos.Lookups;

public class CustomerLookupDto : LookupDto
{
    public string? Code { get; set; }
    public string? Phone { get; set; }
    public Guid? ChartOfAccountId { get; set; }
    public bool IsActive { get; set; }
}

