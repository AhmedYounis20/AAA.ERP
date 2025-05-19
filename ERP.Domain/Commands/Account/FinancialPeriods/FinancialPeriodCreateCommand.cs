using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;

namespace ERP.Domain.Commands.Account.FinancialPeriods;

public class FinancialPeriodCreateCommand : BaseCreateCommand<FinancialPeriod>
{
    public string? YearNumber { get; set; }
    public byte PeriodTypeByMonth { get; set; } = FinancialPeriodType.OneYear;
    public DateTime StartDate { get; set; }
}