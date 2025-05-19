using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;

namespace ERP.Application.Repositories.Account;

public interface IFinancialPeriodRepository : IBaseRepository<FinancialPeriod>
{
    Task<FinancialPeriod?> GetLastFinancialPeriod();
    Task<FinancialPeriod?> GetCurrentFinancialPeroid();
    Task<bool> IsExisted(string? yearNumber);
    Task<List<FinancialPeriod>> GetIntersectedFinancialPeriods(DateTime startDate, DateTime endDate);

}