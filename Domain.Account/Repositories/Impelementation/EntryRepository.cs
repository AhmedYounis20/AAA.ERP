using Domain.Account.DBConfiguration.DbContext;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Repositories.BaseRepositories.Impelementation;
using Domain.Account.Repositories.Interfaces;

namespace Domain.Account.Repositories.Impelementation;

public class EntryRepository : BaseRepository<Entry>, IEntryRepository
{
    public EntryRepository(ApplicationDbContext context) : base(context)
    { }
}