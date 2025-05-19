using ERP.Application.Repositories.Account;
using ERP.Domain.Models.Entities.Account.Entries;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Account;

public class EntryRepository : BaseRepository<Entry>, IEntryRepository
{
    public EntryRepository(ApplicationDbContext context) : base(context)
    { }

    public override async Task<Entry?> Get(Guid id)
    {
        return await _dbSet.Include(e=>e.FinancialPeriod).Include(e => e.EntryAttachments)
            .ThenInclude(e => e.Attachment)
            .Include(e=>e.FinancialTransactions)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public override async Task<IEnumerable<Entry>> Get()
    {
        return await _dbSet.Include(e=>e.FinancialPeriod).Include(e => e.EntryAttachments)
            .ThenInclude(e => e.Attachment)
            .ToListAsync();
    }

    public  async Task<Entry?> Get(Guid id,EntryType entryType)
    {
        return await _dbSet.Include(e => e.FinancialPeriod).Include(e => e.EntryAttachments)
            .ThenInclude(e => e.Attachment)
            .Include(e => e.FinancialTransactions)
            .Where(e => e.Id == id && e.EntryType == entryType)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Entry>> Get(EntryType entryType)
    {
        return await _dbSet.Include(e => e.FinancialPeriod).Include(e => e.EntryAttachments)
            .ThenInclude(e => e.Attachment)
            .Where(e=>e.EntryType == entryType)
            .ToListAsync();
    }

}