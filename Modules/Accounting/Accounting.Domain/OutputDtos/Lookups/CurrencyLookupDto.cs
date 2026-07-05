using AAA.ERP.OutputDtos.BaseDtos;
using Shared.DTOs;

namespace ERP.Domain.OutputDtos.Lookups;

public class CurrencyLookupDto : LookupDto
{
    public decimal ExchangeRate { get; set; }
    public string? Symbol { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}