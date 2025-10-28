using Account.Domain.DTOs;
using Account.Domain.Models.Entities.FinancialPeriods;
using Shared.Application.Repositories;

namespace Account.Application.Repositories;

public interface IFinancialPeriodRepository : IBaseRepository<FinancialPeriod>
{
    Task<FinancialPeriod?> GetLastFinancialPeriod();
    Task<FinancialPeriod?> GetCurrentFinancialPeroid();
    Task<List<FinancialPeriodDto>> GetDtos();
    Task<FinancialPeriodDto?> GetDto(Guid id);
    Task<bool> IsExisted(string? yearNumber);
    Task<List<FinancialPeriod>> GetIntersectedFinancialPeriods(DateTime startDate, DateTime endDate);
    //Task<bool> IsUsedInInventoryTransactions(Guid id);
    Task<bool> IsUsedInEntries(Guid id);
    Task<FinancialPeriodDto> GetNextDefaultdata();
    Task<bool> IsLastFinancialPeriod(Guid id);
}