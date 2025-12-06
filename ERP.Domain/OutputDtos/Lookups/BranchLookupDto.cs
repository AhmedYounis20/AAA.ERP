using Shared.DTOs;

namespace ERP.Domain.OutputDtos.Lookups;

public class BranchLookupDto : LookupDto
{
    public string? Code { get; set; }
    public bool IsActive { get; set; }
}

