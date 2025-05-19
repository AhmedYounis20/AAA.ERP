using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Account.Currencies;
using Shared.Responses;

namespace ERP.Domain.Commands.Account.Currencies;

public class CurrencyUpdateCommand : BaseSettingUpdateCommand<Currency>
{
    public decimal ExchangeRate { get; set; }
    public string? Symbol { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}