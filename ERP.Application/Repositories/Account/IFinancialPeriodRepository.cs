using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Dtos.FinancialPeriods;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;

namespace ERP.Application.Repositories.Account;

public interface IFinancialPeriodRepository : IBaseRepository<FinancialPeriod>
{
    Task<FinancialPeriod?> GetLastFinancialPeriod();
    Task<FinancialPeriod?> GetCurrentFinancialPeroid();
    Task<List<FinancialPeriodDto>> GetDtos();
    Task<FinancialPeriodDto?> GetDto(Guid id);
    Task<bool> IsExisted(string? yearNumber);
    Task<List<FinancialPeriod>> GetIntersectedFinancialPeriods(DateTime startDate, DateTime endDate);
    Task<bool> IsUsedInInventoryTransactions(Guid id);
    Task<bool> IsUsedInEntries(Guid id);
    Task<FinancialPeriodDto> GetNextDefaultdata();
    Task<bool> IsLastFinancialPeriod(Guid id);
}