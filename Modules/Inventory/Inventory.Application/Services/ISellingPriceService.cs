using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.SellingPrices;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Application.Services;

/// <summary>
/// Application service contract for selling price.
/// </summary>
public interface ISellingPriceService : IBaseSettingService<SellingPrice, SellingPriceCreateCommand, SellingPriceUpdateCommand>
{
    Task<ApiResponse<IEnumerable<SellingPriceDto>>> GetDtos();
}
