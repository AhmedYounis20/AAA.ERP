using ERP.BuildingBlocks.Application.Commands.BaseInputModels;

namespace Accounting.Domain.Commands;

public class CurrencyInputModel : BaseSettingInputModel
{
    public decimal ExchangeRate { get; set; }
    public string? Symbol { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}