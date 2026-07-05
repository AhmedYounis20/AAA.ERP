using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Commands.SellingPrices;
using Inventory.Application.Dtos;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.SellingPrices;
using ERP.Infrastructure.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Selling price read queries. Transitional: delegates to <see cref="ISellingPriceService"/> until write methods are removed.
/// </summary>
public class SellingPriceQueries(ISellingPriceService sellingPriceService)
    : BaseSettingQueries<SellingPrice, SellingPriceCreateCommand, SellingPriceUpdateCommand>(sellingPriceService),
      ISellingPriceQueries
{
    public Task<ApiResponse<IEnumerable<SellingPriceDto>>> GetDtos()
        => sellingPriceService.GetDtos();
}
