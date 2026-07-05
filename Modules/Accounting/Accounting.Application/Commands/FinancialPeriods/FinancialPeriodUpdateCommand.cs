using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using Accounting.Domain.Models.Entities.FinancialPeriods;

namespace Accounting.Application.Commands.FinancialPeriods;

public class FinancialPeriodUpdateCommand : BaseUpdateCommand<FinancialPeriod>
{
    public string? YearNumber { get; set; }
    public byte PeriodTypeByMonth { get; set; }
}
