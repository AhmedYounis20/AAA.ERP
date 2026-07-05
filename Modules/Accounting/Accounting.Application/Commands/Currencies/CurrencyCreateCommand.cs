using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Accounting.Domain.Models.Entities.Currencies;

namespace Accounting.Application.Commands.Currencies;

public class CurrencyCreateCommand : BaseSettingCreateCommand<Currency>
{
    public decimal ExchangeRate { get; set; }
    public string? Symbol { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}
