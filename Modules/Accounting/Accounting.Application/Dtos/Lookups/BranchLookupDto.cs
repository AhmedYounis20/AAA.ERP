using ERP.BuildingBlocks.Application.DTOs;

namespace Accounting.Application.Dtos.Lookups;

public class BranchLookupDto : LookupDto
{
    public string? Code { get; set; }
    public bool IsActive { get; set; }
}

