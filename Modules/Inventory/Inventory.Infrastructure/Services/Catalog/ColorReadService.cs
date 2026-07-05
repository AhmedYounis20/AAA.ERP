using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Commands.Colors;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Colors;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Color read queries. Transitional: delegates to <see cref="IColorService"/> until write methods are removed.
/// </summary>
public class ColorReadService(IColorService colorService) : IColorReadService
{
    public Task<ApiResponse<IEnumerable<Color>>> GetAll()
        => colorService.ReadAll();

    public Task<ApiResponse<PaginatedResult<Color>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default)
        => colorService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<Color>> GetById(Guid id)
        => colorService.ReadById(id);

    public Task<ApiResponse<string>> GetNextCode()
        => colorService.GetNextCodeAsync();
}
