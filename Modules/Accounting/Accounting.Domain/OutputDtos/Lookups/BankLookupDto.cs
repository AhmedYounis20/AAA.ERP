using Shared.DTOs;

namespace Accounting.Domain.OutputDtos.Lookups;

public class BankLookupDto : LookupDto
{
    public string? Code { get; set; }
    public Guid? ChartOfAccountId { get; set; }
    public bool IsActive { get; set; }
}

