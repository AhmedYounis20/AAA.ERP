using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;

namespace Domain.Account.Commands.Currencies;

public class CurrencyCreateCommand : BaseSettingCreateCommand<CurrencyCreateCommand>
{
    public decimal ExchangeRate { get; set; }
    public string? Symbol { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}