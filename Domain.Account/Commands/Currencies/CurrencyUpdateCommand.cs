using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Models.Entities.Currencies;
using Shared.Responses;

namespace Domain.Account.Commands.Currencies;

public class CurrencyUpdateCommand : BaseSettingUpdateCommand<Currency>
{
    public decimal ExchangeRate { get; set; }
    public string? Symbol { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}