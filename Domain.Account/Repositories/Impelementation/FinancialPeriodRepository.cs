using AAA.ERP.DBConfiguration.DbContext;
using AAA.ERP.Models.Entities.FinancialPeriods;
using AAA.ERP.Repositories.BaseRepositories.Impelementation;
using AAA.ERP.Repositories.Interfaces;

namespace AAA.ERP.Repositories.Impelementation;

public class FinancialPeriodRepository : BaseRepository<FinancialPeriod>, IFinancialPeriodRepository
{
    DbSet<FinancialPeriod> _dbset;
    public FinancialPeriodRepository(ApplicationDbContext context) : base(context) 
        => _dbset = context.Set<FinancialPeriod>();

    public async Task<FinancialPeriod?> GetCurrentFinancialPeroid()
    {
        return await _dbset.Where(e=>e.StartDate<= DateTime.Now && e.EndDate > DateTime.Now).FirstOrDefaultAsync();
    }

    public async Task<List<FinancialPeriod>> GetIntersectedFinancialPeriods(DateTime startDate, DateTime endDate)
    {
        return await _dbset.Where(e => 
        (e.StartDate <= startDate && e.EndDate >= endDate)
        || (e.StartDate >= startDate && e.EndDate <= endDate)
        ).ToListAsync();
    }

    public async Task<FinancialPeriod?> GetLastFinancialPeriod()
    {
        return await _dbset.OrderByDescending(e=>e.StartDate).FirstOrDefaultAsync();
    }

    public async Task<bool> IsExisted(string? yearNumber)
    {
        return await _dbset.AnyAsync(e=>e.YearNumber == yearNumber);
    }
}