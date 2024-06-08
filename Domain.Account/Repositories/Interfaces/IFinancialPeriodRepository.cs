using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Repositories.BaseRepositories.Interfaces;

namespace Domain.Account.Repositories.Interfaces;

public interface IFinancialPeriodRepository: IBaseRepository<FinancialPeriod>
{
    Task<FinancialPeriod?> GetLastFinancialPeriod();
    Task<FinancialPeriod?> GetCurrentFinancialPeroid();
    Task<bool> IsExisted(string? yearNumber);
    Task<List<FinancialPeriod>> GetIntersectedFinancialPeriods(DateTime startDate, DateTime endDate);

}