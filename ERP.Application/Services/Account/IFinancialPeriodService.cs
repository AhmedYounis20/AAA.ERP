using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Account.FinancialPeriods;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;
using Shared.Responses;

namespace ERP.Application.Services.Account;

public interface IFinancialPeriodService : IBaseService<FinancialPeriod, FinancialPeriodCreateCommand, FinancialPeriodUpdateCommand>
{

    Task<ApiResponse<FinancialPeriod>> GetCurrentFinancailPeriod();
}