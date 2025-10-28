namespace Account.Domain.Models.Entities.Currencies;

public class Currency : BaseSettingEntity
{
    public string? Code { get; set; }
    public decimal ExchangeRate { get; set; }
    public string? Symbol { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}
