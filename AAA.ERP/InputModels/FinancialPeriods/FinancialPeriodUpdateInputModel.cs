using AAA.ERP.InputModels.BaseInputModels;
using AAA.ERP.Models.Entities.FinancialPeriods;

namespace AAA.ERP.InputModels.FinancialPeriods;

public class FinancialPeriodUpdateInputModel : BaseInputModel
{
    public string? YearNumber { get; set; }
}