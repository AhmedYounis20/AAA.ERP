using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.Currencies;
using Domain.Account.Models.Entities.FinancialPeriods;
using Shared.Responses;

namespace Domain.Account.Commands.FinancialPeriods;

public class FinancialPeriodCreateCommand : BaseCreateCommand<ApiResponse<FinancialPeriod>>
{
    public string? YearNumber { get; set; }
    public byte PeriodTypeByMonth { get; set; } = FinancialPeriodType.OneYear;
    public DateTime StartDate { get; set; }
}