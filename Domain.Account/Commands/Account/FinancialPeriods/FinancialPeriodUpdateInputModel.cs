using Domain.Account.InputModels.BaseInputModels;

namespace ERP.Domain.Commands.Account.FinancialPeriods;

public class FinancialPeriodUpdateInputModel : BaseInputModel
{
    public string? YearNumber { get; set; }
}