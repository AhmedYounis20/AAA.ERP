using Accounting.Application.Dtos.Entries;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Application.Services.Entries;
using Accounting.Contracts.Dtos;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Infrastructure.Services.Entries;

/// <summary>
/// Combined entry reads. Transitional: delegates to <see cref="ICombinedEntryService"/>.
/// </summary>
public class CombinedEntryQueries(ICombinedEntryService combinedEntryService, IEntryService entryService)
    : ICombinedEntryQueries
{
    public Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetAll()
        => combinedEntryService.GetComplexEntries();

    public Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default)
        => entryService.GetPaginated(filter, cancellationToken);

    public Task<ApiResponse<ComplexEntryDto>> GetById(Guid id)
        => combinedEntryService.GetComplexEntryById(id);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => combinedEntryService.GetEntryNumber(dateTime);
}
