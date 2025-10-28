using Account.Domain.Models.Entities.FinancialPeriods;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Account.Domain.Commands.FinancialPeriods;

public class FinancialPeriodCreateCommand : BaseCreateCommand<FinancialPeriod>
{
    public string? YearNumber { get; set; }
    public byte PeriodTypeByMonth { get; set; } = FinancialPeriodType.OneYear;
    public DateTime StartDate { get; set; }
}