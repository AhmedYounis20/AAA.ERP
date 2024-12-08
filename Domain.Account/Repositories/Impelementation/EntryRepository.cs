using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Repositories.BaseRepositories.Impelementation;
using Domain.Account.Repositories.BaseRepositories.Interfaces;
using Domain.Account.Repositories.Interfaces;

namespace Domain.Account.Repositories.Impelementation;

public class EntryRepository : BaseRepository<Entry>, IEntryRepository
{
    public EntryRepository(ApplicationDbContext context) : base(context)
    { }

    public override async Task<Entry?> Get(Guid id)
    {
        return await _dbSet.Include(e=>e.FinancialPeriod).Include(e => e.EntryAttachments)
            .ThenInclude(e => e.Attachment)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Entry>> Get()
    {
        return await _dbSet.Include(e=>e.FinancialPeriod).Include(e => e.EntryAttachments)
            .ThenInclude(e => e.Attachment)
            .ToListAsync();
    }
}