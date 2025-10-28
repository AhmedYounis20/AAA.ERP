using Account.Domain.Models.Entities.Currencies;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Account.Domain.Commands.Currencies;

public class CurrencyUpdateCommand : BaseSettingUpdateCommand<Currency>
{
    public string Code { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}