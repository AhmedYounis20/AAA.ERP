using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Domain.Models.Dtos.FinancialPeriods;
using Accounting.Domain.Models.Entities.FinancialPeriods;

namespace Accounting.Application.Repositories;

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
