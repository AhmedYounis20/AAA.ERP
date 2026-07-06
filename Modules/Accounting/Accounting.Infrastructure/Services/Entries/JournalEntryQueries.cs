using Accounting.Application.Dtos.Entries;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Application.Services.Entries;
using Accounting.Contracts.Dtos;
using Accounting.Domain.Models.Entities.Entries;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services.Entries;

/// <summary>
/// Journal entry reads.
/// </summary>
public class JournalEntryQueries(IEntryService entryService) : IJournalEntryQueries
{
    public Task<ApiResponse<IEnumerable<EntryDto>>> GetAll()
        => entryService.Get(EntryType.Journal);

    public Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default)
        => entryService.GetPaginated(filter, cancellationToken);

    public Task<ApiResponse<EntryDto>> GetById(Guid id)
        => entryService.Get(id, EntryType.Journal);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => entryService.GetEntryNumber(dateTime);
}
