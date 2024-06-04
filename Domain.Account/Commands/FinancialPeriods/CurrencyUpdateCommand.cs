using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

namespace Domain.Account.Commands.Currencies;

public class CurrencyUpdateCommand : BaseSettingUpdateCommand<CurrencyUpdateCommand>
{
    public decimal ExchangeRate { get; set; }
    public string? Symbol { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}