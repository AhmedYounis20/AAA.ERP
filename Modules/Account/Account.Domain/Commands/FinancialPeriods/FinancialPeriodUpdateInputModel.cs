using Shared.Domain.Commands.BaseInputModels;

namespace Account.Domain.Commands.FinancialPeriods;

public class FinancialPeriodUpdateInputModel : BaseInputModel
{
    public string? YearNumber { get; set; }
}