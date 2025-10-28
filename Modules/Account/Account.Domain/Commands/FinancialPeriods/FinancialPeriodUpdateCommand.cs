using Account.Domain.Models.Entities.FinancialPeriods;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Account.Domain.Commands.FinancialPeriods;

public class FinancialPeriodUpdateCommand : BaseUpdateCommand<FinancialPeriod>
{
    public string? YearNumber { get; set; }
    public byte PeriodTypeByMonth { get; set; }

}