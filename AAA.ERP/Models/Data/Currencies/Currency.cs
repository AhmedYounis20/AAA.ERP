using AAA.ERP.Models.BaseEntities;

namespace AAA.ERP.Models.Data.Currencies;

public class Currency : BaseSettingEntity {

    public decimal ExchangeRate { get; set; }
    public string? Symbol { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}