using Domain.Account.Models.Entities.Entries;
using ERP.Application.Repositories.BaseRepositories;

namespace ERP.Application.Repositories;

public interface IEntryRepository : IBaseRepository<Entry> {
    Task<IEnumerable<Entry>> Get(EntryType entryType);
    Task<Entry?> Get(Guid id, EntryType entryType);
}