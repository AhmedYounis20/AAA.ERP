using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.Application.Repositories.Account;

public interface IEntryRepository : IBaseRepository<Entry>
{
    Task<IEnumerable<Entry>> Get(EntryType entryType);
    Task<Entry?> Get(Guid id, EntryType entryType);
}