using Shared.DTOs;

namespace ERP.Domain.OutputDtos.Lookups;

public class SellingPriceLookupDto : LookupDto
{
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}

