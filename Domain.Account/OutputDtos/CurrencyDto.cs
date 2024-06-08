using AAA.ERP.OutputDtos.BaseDtos;

namespace AAA.ERP.OutputDtos;

public class CurrencyDto : BaseSettingDto
{
    public decimal ExchangeRate { get; set; }
    public string? Symbol { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}