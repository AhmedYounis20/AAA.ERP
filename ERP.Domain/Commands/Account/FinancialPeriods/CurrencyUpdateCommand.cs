using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;
using Shared.Responses;

namespace ERP.Domain.Commands.Account.FinancialPeriods;

public class FinancialPeriodUpdateCommand : BaseUpdateCommand<FinancialPeriod>
{
    public string? YearNumber { get; set; }
}