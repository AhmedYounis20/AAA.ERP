using Account.Domain.Models.Entities.Entries;
using Shared.Application.Repositories;

namespace Account.Application.Repositories;

public interface IEntryRepository : IBaseRepository<Entry>
{
    Task<IEnumerable<Entry>> Get(EntryType entryType);
    Task<Entry?> Get(Guid id, EntryType entryType);
    Task<bool> EntryAfterThisDateExist(DateTime date);
}