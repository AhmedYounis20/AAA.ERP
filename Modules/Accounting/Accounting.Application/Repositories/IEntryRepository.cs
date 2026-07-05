using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Account.Entries;

namespace Accounting.Application.Repositories;

public interface IEntryRepository : IBaseRepository<Entry>
{
    Task<IEnumerable<Entry>> Get(EntryType entryType);
    Task<Entry?> Get(Guid id, EntryType entryType);
    Task<bool> EntryAfterThisDateExist(DateTime date);
}