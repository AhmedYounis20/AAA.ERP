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
/// Combined entry reads.
/// </summary>
public class CombinedEntryQueries(IComplexEntryService complexEntryService, IEntryService entryService)
    : ICombinedEntryQueries
{
    public Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetAll()
        => complexEntryService.GetComplexEntries(EntryType.Combined);

    public Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default)
        => entryService.GetPaginated(filter, cancellationToken);

    public Task<ApiResponse<ComplexEntryDto>> GetById(Guid id)
        => complexEntryService.GetComplexEntryById(id, EntryType.Combined);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => complexEntryService.GetEntryNumber(dateTime);
}
