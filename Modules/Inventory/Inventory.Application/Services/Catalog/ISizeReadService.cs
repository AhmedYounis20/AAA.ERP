using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for size GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ISizeReadService
{
    Task<ApiResponse<IEnumerable<Size>>> GetAll();

    Task<ApiResponse<PaginatedResult<Size>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<Size>> GetById(Guid id);

    Task<ApiResponse<string>> GetNextCode();
}
