using AAA.ERP.InputModels.BaseInputModels;
using AAA.ERP.Models.Entities.FinancialPeriods;

namespace AAA.ERP.InputModels.FinancialPeriods;

public class FinancialPeriodInputModel : BaseInputModel
{
    public string? YearNumber { get; set; }
    public byte PeriodTypeByMonth { get; set; } = FinancialPeriodType.OneYear;
    public DateTime StartDate { get; set; }
}