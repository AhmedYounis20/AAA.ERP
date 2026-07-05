using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Commands.Colors;
using Inventory.Domain.Models.Entities.Colors;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for color GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IColorReadService
{
    Task<ApiResponse<IEnumerable<Color>>> GetAll();

    Task<ApiResponse<PaginatedResult<Color>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<Color>> GetById(Guid id);

    Task<ApiResponse<string>> GetNextCode();
}
