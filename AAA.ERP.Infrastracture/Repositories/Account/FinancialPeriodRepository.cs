using ERP.Application.Repositories.Account;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Account;

public class FinancialPeriodRepository : BaseRepository<FinancialPeriod>, IFinancialPeriodRepository
{
    DbSet<FinancialPeriod> _dbset;
    public FinancialPeriodRepository(ApplicationDbContext context) : base(context)
        => _dbset = context.Set<FinancialPeriod>();

    public async Task<FinancialPeriod?> GetCurrentFinancialPeroid()
    {
        return await _dbset.Where(e => e.StartDate <= DateTime.Now && e.EndDate > DateTime.Now).FirstOrDefaultAsync();
    }

    public async Task<List<FinancialPeriod>> GetIntersectedFinancialPeriods(DateTime startDate, DateTime endDate)
    {
        return await _dbset.Where(e =>
        e.StartDate <= startDate && e.EndDate >= endDate
        || e.StartDate >= startDate && e.EndDate <= endDate
        ).ToListAsync();
    }

    public async Task<FinancialPeriod?> GetLastFinancialPeriod()
    {
        return await _dbset.OrderByDescending(e => e.StartDate).FirstOrDefaultAsync();
    }

    public async Task<bool> IsExisted(string? yearNumber)
    {
        return await _dbset.AnyAsync(e => e.YearNumber == yearNumber);
    }
}