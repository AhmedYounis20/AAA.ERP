using Domain.Account.Models.Entities.FinancialPeriods;
using ERP.Application.Repositories.BaseRepositories;

namespace ERP.Application.Repositories;

public interface IFinancialPeriodRepository: IBaseRepository<FinancialPeriod>
{
    Task<FinancialPeriod?> GetLastFinancialPeriod();
    Task<FinancialPeriod?> GetCurrentFinancialPeroid();
    Task<bool> IsExisted(string? yearNumber);
    Task<List<FinancialPeriod>> GetIntersectedFinancialPeriods(DateTime startDate, DateTime endDate);

}