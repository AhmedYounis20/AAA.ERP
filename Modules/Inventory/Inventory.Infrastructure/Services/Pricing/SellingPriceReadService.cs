using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Selling price read queries. Transitional: delegates to <see cref="ISellingPriceService"/> until write methods are removed.
/// </summary>
public class SellingPriceReadService(ISellingPriceService sellingPriceService)
    : ISellingPriceReadService
{
    public Task<ApiResponse<IEnumerable<SellingPriceDto>>> GetDtos()
        => sellingPriceService.GetDtos();

    public Task<ApiResponse<PaginatedResult<SellingPrice>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default)
        => sellingPriceService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<SellingPrice>> GetById(Guid id)
        => sellingPriceService.ReadById(id);
}
