using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Packing unit read queries. Transitional: delegates to <see cref="IPackingUnitService"/> until write methods are removed.
/// </summary>
public class PackingUnitReadService(IPackingUnitService packingUnitService) : IPackingUnitReadService
{
    public Task<ApiResponse<IEnumerable<PackingUnit>>> GetAll()
        => packingUnitService.ReadAll();

    public Task<ApiResponse<PaginatedResult<PackingUnit>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default)
        => packingUnitService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<PackingUnit>> GetById(Guid id)
        => packingUnitService.ReadById(id);
}
