using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Models.Entities.FinancialPeriods;
using Shared.Responses;

namespace Domain.Account.Commands.FinancialPeriods;

public class FinancialPeriodUpdateCommand : BaseUpdateCommand<ApiResponse<FinancialPeriod>>
{
    public string? YearNumber { get; set; }
}