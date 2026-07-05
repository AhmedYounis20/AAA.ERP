using Shared.DTOs;

namespace ERP.Domain.OutputDtos.Lookups;

public class SupplierLookupDto : LookupDto
{
    public string? Code { get; set; }
    public string? Phone { get; set; }
    public Guid? ChartOfAccountId { get; set; }
    public bool IsActive { get; set; }
}

