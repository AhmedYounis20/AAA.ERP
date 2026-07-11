using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using Accounting.Domain.Models.Entities.Currencies;

namespace Accounting.Application.Commands.Currencies;

public class CurrencyUpdateCommand : BaseSettingUpdateCommand<Currency>
{
    public decimal ExchangeRate { get; set; }
    public string? Symbol { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}
