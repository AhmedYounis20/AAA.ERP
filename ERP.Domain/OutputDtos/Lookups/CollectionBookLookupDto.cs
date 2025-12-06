using Shared.DTOs;

namespace ERP.Domain.OutputDtos.Lookups;

public class CollectionBookLookupDto : LookupDto
{
    public string? Code { get; set; }
    public bool IsActive { get; set; }
}

