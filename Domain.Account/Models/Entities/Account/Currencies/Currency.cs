using Shared.BaseEntities;

namespace ERP.Domain.Models.Entities.Account.Currencies;

public class Currency : BaseSettingEntity
{

    public decimal ExchangeRate { get; set; }
    public string? Symbol { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}