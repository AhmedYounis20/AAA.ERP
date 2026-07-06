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
/// Journal entry reads. Transitional: delegates to <see cref="IJournalEntryService"/>.
/// </summary>
public class JournalEntryQueries(IJournalEntryService journalEntryService, IEntryService entryService)
    : IJournalEntryQueries
{
    public Task<ApiResponse<IEnumerable<EntryDto>>> GetAll()
        => journalEntryService.GetDto();

    public Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default)
        => entryService.GetPaginated(filter, cancellationToken);

    public Task<ApiResponse<EntryDto>> GetById(Guid id)
        => journalEntryService.GetDto(id);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => journalEntryService.GetEntryNumber(dateTime);
}
