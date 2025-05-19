using Domain.Account.Commands.FinancialPeriods;
using Domain.Account.Models.Entities.FinancialPeriods;
using ERP.Application.Services.BaseServices;
using Shared.Responses;

namespace ERP.Application.Services.Account;

public interface IFinancialPeriodService : IBaseService<FinancialPeriod, FinancialPeriodCreateCommand, FinancialPeriodUpdateCommand>
{

    Task<ApiResponse<FinancialPeriod>> GetCurrentFinancailPeriod();
}