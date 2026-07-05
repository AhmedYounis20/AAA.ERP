using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for packing unit GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IPackingUnitReadService
{
    Task<ApiResponse<IEnumerable<PackingUnit>>> GetAll();

    Task<ApiResponse<PaginatedResult<PackingUnit>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<PackingUnit>> GetById(Guid id);
}
