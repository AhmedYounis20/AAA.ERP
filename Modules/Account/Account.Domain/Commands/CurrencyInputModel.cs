using Shared.Domain.Commands.BaseInputModels;

namespace Account.Domain.Commands;

public class CurrencyInputModel : BaseSettingInputModel
{
    public decimal ExchangeRate { get; set; }
    public string? Symbol { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}