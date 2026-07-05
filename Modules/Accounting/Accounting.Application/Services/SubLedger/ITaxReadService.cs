using Accounting.Application.Commands.Taxes;
using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Entities;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for tax GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ITaxReadService
{
    Task<ApiResponse<IEnumerable<Tax>>> GetAll();

    Task<ApiResponse<PaginatedResult<Tax>>> GetPaginated(
        SubLeadgerFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<Tax>> GetById(Guid id);

    Task<ApiResponse<TaxCreateCommand>> GetNextSubLedgers(Guid? parentId);

    Task<ApiResponse<IEnumerable<Tax>>> GetLevel(int level = 0);

    Task<ApiResponse<IEnumerable<Tax>>> GetChildren(Guid parentId, int level = 0);
}
