using Domain.Account.InputModels.BaseInputModels;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;

namespace Accounting.Application.Commands.FinancialPeriods;

public class FinancialPeriodInputModel : BaseInputModel
{
    public string? YearNumber { get; set; }
    public byte PeriodTypeByMonth { get; set; } = FinancialPeriodType.OneYear;
    public DateTime StartDate { get; set; }
}
