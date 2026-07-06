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
/// Complex entry reads.
/// </summary>
public class ComplexEntryQueries(IComplexEntryService complexEntryService, IEntryService entryService)
    : IComplexEntryQueries
{
    public Task<ApiResponse<IEnumerable<Entry>>> GetAll()
        => entryService.ReadAll();

    public Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default)
        => entryService.GetPaginated(filter, cancellationToken);

    public Task<ApiResponse<ComplexEntryDto>> GetById(Guid id)
        => complexEntryService.GetComplexEntryById(id);

    public Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime)
        => complexEntryService.GetEntryNumber(dateTime);
}
