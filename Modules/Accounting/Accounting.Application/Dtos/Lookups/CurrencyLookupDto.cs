using ERP.BuildingBlocks.Application.Dtos.BaseDtos;
using ERP.BuildingBlocks.Application.DTOs;

namespace Accounting.Application.Dtos.Lookups;

public class CurrencyLookupDto : LookupDto
{
    public decimal ExchangeRate { get; set; }
    public string? Symbol { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}