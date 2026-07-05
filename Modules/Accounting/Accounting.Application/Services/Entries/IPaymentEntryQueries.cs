using Accounting.Application.Dtos.Entries;
using Accounting.Application.Dtos.Filters;
using Accounting.Contracts.Dtos;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Accounting.Domain.Models.Entities.Entries;

namespace Accounting.Application.Services.Entries;

/// <summary>
/// Read-side contract for payment entry GET endpoints (hybrid CQRS).
/// </summary>
public interface IPaymentEntryQueries
{
    Task<ApiResponse<IEnumerable<ComplexEntryDto>>> GetAll();

    Task<ApiResponse<PaginatedResult<Entry>>> GetPaginated(
        EntryFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<ComplexEntryDto>> GetById(Guid id);

    Task<ApiResponse<EntryNumberDto>> GetEntryNumber(DateTime dateTime);
}
