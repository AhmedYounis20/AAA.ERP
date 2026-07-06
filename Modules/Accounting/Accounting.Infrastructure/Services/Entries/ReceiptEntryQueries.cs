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
/// Receipt entry reads.
/// </summary>
public class ReceiptEntryQueries(IComplexEntryService complexEntryService, IEntryService entryService)
    : IReceiptEntryQueries
{
    public Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetAll()
        => complexEntryService.GetComplexEntries(EntryType.Receipt);

    public Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default)
        => entryService.GetPaginated(filter, cancellationToken);

    public Task<ApiResponse<ComplexEntryDto>> GetById(Guid id)
        => complexEntryService.GetComplexEntryById(id, EntryType.Receipt);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => complexEntryService.GetEntryNumber(dateTime);
}
