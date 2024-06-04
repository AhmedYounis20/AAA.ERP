using AAA.ERP.Models.Entities.FinancialPeriods;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;

namespace AAA.ERP.Repositories.Interfaces;

public interface IFinancialPeriodRepository: IBaseRepository<FinancialPeriod>
{
    Task<FinancialPeriod?> GetLastFinancialPeriod();
    Task<FinancialPeriod?> GetCurrentFinancialPeroid();
    Task<bool> IsExisted(string? yearNumber);
    Task<List<FinancialPeriod>> GetIntersectedFinancialPeriods(DateTime startDate, DateTime endDate);

}