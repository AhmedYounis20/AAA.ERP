using Accounting.Application.Repositories;
using Accounting.Application.Dtos.FinancialPeriods;
using Accounting.Domain.Models.Entities.Entries;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using ERP.BuildingBlocks.Persistence.Repositories;
using Inventory.Contracts.Integration;

namespace Accounting.Infrastructure.Repositories;

public class FinancialPeriodRepository : BaseRepository<FinancialPeriod>, IFinancialPeriodRepository
{
    private readonly DbSet<FinancialPeriod> _dbset;
    private readonly IApplicationDbContext _context;
    private readonly IFinancialPeriodInventoryUsageQueries _usageQueries;

    public FinancialPeriodRepository(IApplicationDbContext context, IFinancialPeriodInventoryUsageQueries usageQueries) : base(context)
    {
        _dbset = context.Set<FinancialPeriod>();
        _context = context;
        _usageQueries = usageQueries;
    }

    public async Task<FinancialPeriod?> GetCurrentFinancialPeroid()
        => await _dbset.Where(e => e.StartDate <= DateTime.Now && e.EndDate > DateTime.Now).FirstOrDefaultAsync();

    public async Task<List<FinancialPeriod>> GetIntersectedFinancialPeriods(DateTime startDate, DateTime endDate)
        => await _dbset.Where(e =>
            e.StartDate <= startDate && e.EndDate >= endDate
            || e.StartDate >= startDate && e.EndDate <= endDate
        ).ToListAsync();

    public async Task<FinancialPeriod?> GetLastFinancialPeriod()
        => await _dbset.OrderByDescending(e => e.StartDate).FirstOrDefaultAsync();

    public async Task<FinancialPeriodDto?> GetDto(Guid id)
    {
        var transaction = await _dbset.FirstOrDefaultAsync(e => e.Id == id);
        if (transaction is null)
            return null;

        return await MapToDto(transaction);
    }

    public async Task<FinancialPeriodDto> GetNextDefaultdata()
    {
        var lastPeriod = await GetLastFinancialPeriod();
        var periodInMonths = lastPeriod == null ? FinancialPeriodType.OneYear : lastPeriod.PeriodTypeByMonth;
        var startDate = lastPeriod == null ? DateTime.Now : lastPeriod.EndDate.AddTicks(1);
        return new FinancialPeriodDto
        {
            StartDate = startDate,
            PeriodTypeByMonth = periodInMonths,
            EndDate = startDate.AddMonths(periodInMonths).AddTicks(-1)
        };
    }

    public async Task<List<FinancialPeriodDto>> GetDtos()
    {
        var periods = await _dbset.OrderBy(e => e.StartDate).ToListAsync();
        var dtos = new List<FinancialPeriodDto>();
        foreach (var transaction in periods)
            dtos.Add(await MapToDto(transaction, includeNameEditable: false));

        return dtos;
    }

    public Task<bool> IsExisted(string? yearNumber)
        => _dbset.AnyAsync(e => e.YearNumber == yearNumber);

    public Task<bool> IsUsedInEntries(Guid id)
        => _context.Set<Entry>().AnyAsync(e => e.FinancialPeriodId == id);

    public Task<bool> IsUsedInInventoryTransactions(Guid id)
        => _usageQueries.IsUsedInInventoryTransactionsAsync(id);

    public async Task<bool> IsLastFinancialPeriod(Guid id)
    {
        var period = await _context.Set<FinancialPeriod>().OrderBy(e => e.StartDate).LastOrDefaultAsync();
        return period != null && period.Id == id;
    }

    private async Task<FinancialPeriodDto> MapToDto(FinancialPeriod transaction, bool includeNameEditable = true)
    {
        var usedInEntries = await IsUsedInEntries(transaction.Id);
        var usedInInventoryTransaction = await IsUsedInInventoryTransactions(transaction.Id);
        var hasCommingPeriod = await _dbset.AnyAsync(e => e.StartDate > transaction.StartDate);

        var dto = new FinancialPeriodDto
        {
            Id = transaction.Id,
            StartDate = transaction.StartDate,
            EndDate = transaction.EndDate,
            PeriodTypeByMonth = transaction.PeriodTypeByMonth,
            YearNumber = transaction.YearNumber,
            IsEditable = !(hasCommingPeriod || transaction.EndDate < DateTime.Now),
            IsDeletable = !(hasCommingPeriod || transaction.EndDate < DateTime.Now || usedInEntries || usedInInventoryTransaction)
        };

        if (includeNameEditable)
            dto.IsNameEditable = !(usedInEntries || usedInInventoryTransaction);

        return dto;
    }
}
