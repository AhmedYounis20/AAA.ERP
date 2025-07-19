using ERP.Application.Repositories.Account;
using ERP.Domain.Models.Dtos.FinancialPeriods;
using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;
using ERP.Domain.Models.Entities.Inventory.InventoryTransactions;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Account;

public class FinancialPeriodRepository : BaseRepository<FinancialPeriod>, IFinancialPeriodRepository
{
    DbSet<FinancialPeriod> _dbset;
    ApplicationDbContext _context;
    public FinancialPeriodRepository(ApplicationDbContext context) : base(context)
    {
        _dbset = context.Set<FinancialPeriod>();
        _context = context;
    }

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
    public async Task<FinancialPeriodDto?> GetDto(Guid id)
    {
        var query = await (from transaction in _dbSet

                           let usedInEntries = _context.Set<Entry>().Any(e => e.FinancialPeriodId == transaction.Id)
                           let usedInInventoryTransaction = _context.Set<InventoryTransaction>().Any(e => e.FinancialPeriodId == transaction.Id)
                           let hasCommingPeriod = _dbSet.Any(e => e.StartDate > transaction.StartDate)
                           select new FinancialPeriodDto
                           {
                               Id = transaction.Id,
                               StartDate = transaction.StartDate,
                               EndDate = transaction.EndDate,
                               PeriodTypeByMonth = transaction.PeriodTypeByMonth,
                               YearNumber = transaction.YearNumber,
                               IsEditable = !(hasCommingPeriod || transaction.EndDate < DateTime.Now),
                               IsDeletable = !(hasCommingPeriod || transaction.EndDate < DateTime.Now || usedInEntries || usedInInventoryTransaction),
                               IsNameEditable = !(usedInEntries || usedInInventoryTransaction)
                           }).OrderBy(e => e.StartDate).FirstOrDefaultAsync(e => e.Id == id);

        return query;
    }

    public async Task<FinancialPeriodDto> GetNextDefaultdata()
    {
        var lastPeriod = await GetLastFinancialPeriod();
        var periodInMonths = lastPeriod == null ? FinancialPeriodType.OneYear : lastPeriod.PeriodTypeByMonth;
        var startDate = lastPeriod == null ? DateTime.Now : lastPeriod.EndDate.AddTicks(1);
        var inputModel = new FinancialPeriodDto
        {
            StartDate = startDate,
            PeriodTypeByMonth = periodInMonths,
            EndDate = startDate.AddMonths(periodInMonths).AddTicks(-1)
        };

        return inputModel;
    }

    public async Task<List<FinancialPeriodDto>> GetDtos()
    {
        var query = await (from transaction in _dbSet

                           let usedInEntries = _context.Set<Entry>().Any(e => e.FinancialPeriodId == transaction.Id)
                           let usedInInventoryTransaction = _context.Set<InventoryTransaction>().Any(e => e.FinancialPeriodId == transaction.Id)
                           let hasCommingPeriod = _dbSet.Any(e => e.StartDate > transaction.StartDate)
                           select new FinancialPeriodDto
                           {
                               Id = transaction.Id,
                               StartDate = transaction.StartDate,
                               EndDate = transaction.EndDate,
                               PeriodTypeByMonth = transaction.PeriodTypeByMonth,
                               YearNumber = transaction.YearNumber,
                               IsEditable = !(hasCommingPeriod || transaction.EndDate < DateTime.Now),
                               IsDeletable = !(hasCommingPeriod || transaction.EndDate < DateTime.Now || usedInEntries || usedInInventoryTransaction)
                           }).OrderBy(e => e.StartDate).ToListAsync();

        return query;
    }

    public async Task<bool> IsExisted(string? yearNumber)
    {
        return await _dbset.AnyAsync(e => e.YearNumber == yearNumber);
    }

    public async Task<bool> IsUsedInEntries(Guid id)
    {
        return await _context.Set<Entry>().AnyAsync(e => e.FinancialPeriodId == id);
    }

    public async Task<bool> IsUsedInInventoryTransactions(Guid id)
    {
        return await _context.Set<InventoryTransaction>().AnyAsync(e => e.FinancialPeriodId == id);
    }


    public async Task<bool> IsLastFinancialPeriod(Guid id)
    {
        var period = await _context.Set<FinancialPeriod>().OrderBy(e => e.StartDate).LastOrDefaultAsync();
        return period != null && period.Id == id;
    }
}