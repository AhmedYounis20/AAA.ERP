using ERP.Application.Services.BaseServices;
using Inventory.Application.Commands.SellingPrices;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;

namespace Inventory.Application.Services;

public interface ISellingPriceService : IBaseSettingService<SellingPrice, SellingPriceCreateCommand, SellingPriceUpdateCommand>
{
    Task<ApiResponse<IEnumerable<SellingPriceDto>>> GetDtos();
}
