using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.SellingPrices;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Application.Services;

public interface ISellingPriceService : IBaseSettingService<SellingPrice, SellingPriceCreateCommand, SellingPriceUpdateCommand>
{
    Task<ApiResponse<IEnumerable<SellingPriceDto>>> GetDtos();
}
