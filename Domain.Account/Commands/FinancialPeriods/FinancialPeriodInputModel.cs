using Domain.Account.InputModels.BaseInputModels;
using Domain.Account.Models.Entities.FinancialPeriods;

namespace Domain.Account.InputModels.FinancialPeriods;

public class FinancialPeriodInputModel : BaseInputModel
{
    public string? YearNumber { get; set; }
    public byte PeriodTypeByMonth { get; set; } = FinancialPeriodType.OneYear;
    public DateTime StartDate { get; set; }
}