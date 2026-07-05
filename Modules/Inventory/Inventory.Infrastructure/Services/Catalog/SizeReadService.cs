using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Size read queries. Transitional: delegates to <see cref="ISizeService"/> until write methods are removed.
/// </summary>
public class SizeReadService(ISizeService sizeService) : ISizeReadService
{
    public Task<ApiResponse<IEnumerable<Size>>> GetAll()
        => sizeService.ReadAll();

    public Task<ApiResponse<PaginatedResult<Size>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default)
        => sizeService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<Size>> GetById(Guid id)
        => sizeService.ReadById(id);

    public Task<ApiResponse<string>> GetNextCode()
        => sizeService.GetNextCodeAsync();
}
