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
/// Opening entry reads. Transitional: delegates to <see cref="IOpeningEntryService"/>.
/// </summary>
public class OpeningEntryQueries(IOpeningEntryService openingEntryService, IEntryService entryService)
    : IOpeningEntryQueries
{
    public Task<ApiResponse<IEnumerable<EntryDto>>> GetAll()
        => openingEntryService.GetDto();

    public Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default)
        => entryService.GetPaginated(filter, cancellationToken);

    public Task<ApiResponse<EntryDto>> GetById(Guid id)
        => openingEntryService.GetDto(id);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => openingEntryService.GetEntryNumber(dateTime);
}
