using ERP.BuildingBlocks.Application.DTOs;

namespace Inventory.Application.Dtos.Lookups;

public class SellingPriceLookupDto : LookupDto
{
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}

