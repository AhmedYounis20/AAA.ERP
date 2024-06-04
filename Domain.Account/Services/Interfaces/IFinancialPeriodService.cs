using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Services.BaseServices.interfaces;
using Shared.Responses;

namespace Domain.Account.Services.Interfaces;

public interface IFinancialPeriodService: IBaseService<FinancialPeriod>
{

    Task<ApiResponse<FinancialPeriod>> GetCurrentFinancailPeriod();
}