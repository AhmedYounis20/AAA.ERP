using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;

namespace Accounting.Application.Commands.FinancialPeriods;

public class FinancialPeriodUpdateCommand : BaseUpdateCommand<FinancialPeriod>
{
    public string? YearNumber { get; set; }
    public byte PeriodTypeByMonth { get; set; }
}
