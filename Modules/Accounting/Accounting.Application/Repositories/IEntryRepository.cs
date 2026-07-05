using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Application.Repositories;

/// <summary>
/// Repository contract for entry persistence.
/// </summary>
public interface IEntryRepository : IBaseRepository<Entry>
{
    Task<IEnumerable<Entry>> Get(EntryType entryType);
    /// <summary>get operation.</summary>
    Task<Entry?> Get(Guid id, EntryType entryType);
    /// <summary>entry after this date exist operation.</summary>
    Task<bool> EntryAfterThisDateExist(DateTime date);
}
