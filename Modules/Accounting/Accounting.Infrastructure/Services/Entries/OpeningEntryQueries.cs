using Accounting.Application.Dtos.Entries;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Application.Services.Entries;
using Accounting.Contracts.Dtos;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Services.Entries;

/// <summary>
/// Opening entry reads.
/// </summary>
public class OpeningEntryQueries(IEntryService entryService) : IOpeningEntryQueries
{
    public Task<ApiResponse<IEnumerable<EntryDto>>> GetAll()
        => entryService.Get(EntryType.Opening);

    public Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        filter.EntryType ??= (int)EntryType.Opening;
        return entryService.GetPaginated(filter, cancellationToken);
    }

    public Task<ApiResponse<EntryDto>> GetById(Guid id)
        => entryService.Get(id, EntryType.Opening);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => entryService.GetEntryNumber(dateTime);
}
