using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Application.Dtos.FinancialPeriods;
using Accounting.Domain.Models.Entities.FinancialPeriods;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for financial period persistence.
/// </summary>
public interface IFinancialPeriodRepository : IBaseRepository<FinancialPeriod>
{
    /// <summary>Gets last financial period.</summary>
    Task<FinancialPeriod?> GetLastFinancialPeriod();
    /// <summary>Gets current financial peroid.</summary>
    Task<FinancialPeriod?> GetCurrentFinancialPeroid();
    Task<List<FinancialPeriodDto>> GetDtos();
    /// <summary>Gets dto.</summary>
    Task<FinancialPeriodDto?> GetDto(Guid id);
    /// <summary>is existed operation.</summary>
    Task<bool> IsExisted(string? yearNumber);
    Task<List<FinancialPeriod>> GetIntersectedFinancialPeriods(DateTime startDate, DateTime endDate);
    /// <summary>is used in inventory transactions operation.</summary>
    Task<bool> IsUsedInInventoryTransactions(Guid id);
    /// <summary>is used in entries operation.</summary>
    Task<bool> IsUsedInEntries(Guid id);
    /// <summary>Gets next defaultdata.</summary>
    Task<FinancialPeriodDto> GetNextDefaultdata();
    /// <summary>is last financial period operation.</summary>
    Task<bool> IsLastFinancialPeriod(Guid id);
}
