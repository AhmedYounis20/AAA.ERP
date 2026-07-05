using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using Accounting.Domain.Models.Entities.FinancialPeriods;

namespace Accounting.Application.Commands.FinancialPeriods;

public class FinancialPeriodCreateCommand : BaseCreateCommand<FinancialPeriod>
{
    public string? YearNumber { get; set; }
    public byte PeriodTypeByMonth { get; set; } = FinancialPeriodType.OneYear;
    public DateTime StartDate { get; set; }
}
