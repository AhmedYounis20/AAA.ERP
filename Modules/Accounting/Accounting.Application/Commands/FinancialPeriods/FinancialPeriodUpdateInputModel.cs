using ERP.BuildingBlocks.Application.Commands.BaseInputModels;

namespace Accounting.Application.Commands.FinancialPeriods;

public class FinancialPeriodUpdateInputModel : BaseInputModel
{
    public string? YearNumber { get; set; }
}
