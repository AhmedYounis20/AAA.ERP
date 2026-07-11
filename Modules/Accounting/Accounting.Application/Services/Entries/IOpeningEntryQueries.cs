using Accounting.Application.Dtos.Entries;
using Accounting.Application.Dtos.Filters;
using Accounting.Contracts.Dtos;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Application.Services.Entries;

/// <summary>
/// Read-side contract for opening entry GET endpoints (hybrid CQRS).
/// </summary>
public interface IOpeningEntryQueries
{
    Task<ApiResponse<IEnumerable<EntryDto>>> GetAll();

    Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<EntryDto>> GetById(Guid id);

    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
}
