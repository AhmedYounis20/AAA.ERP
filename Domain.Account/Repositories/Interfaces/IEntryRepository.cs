using Domain.Account.Models.Entities.Entries;
using Domain.Account.Repositories.BaseRepositories.Interfaces;

namespace Domain.Account.Repositories.Interfaces;

public interface IEntryRepository : IBaseRepository<Entry> {
    Task<IEnumerable<Entry>> Get(EntryType entryType);
    Task<Entry?> Get(Guid id, EntryType entryType);
}