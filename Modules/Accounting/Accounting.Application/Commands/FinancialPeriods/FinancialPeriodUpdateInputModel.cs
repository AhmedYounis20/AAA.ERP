using Domain.Account.InputModels.BaseInputModels;

namespace Accounting.Application.Commands.FinancialPeriods;

public class FinancialPeriodUpdateInputModel : BaseInputModel
{
    public string? YearNumber { get; set; }
}
