using Domain.Account.InputModels.BaseInputModels;

namespace Domain.Account.InputModels.FinancialPeriods;

public class FinancialPeriodUpdateInputModel : BaseInputModel
{
    public string? YearNumber { get; set; }
}