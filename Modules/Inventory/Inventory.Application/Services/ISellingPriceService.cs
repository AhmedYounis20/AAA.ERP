using Inventory.Domain.Models.Entities.SellingPrices;
using Inventory.Domain.Commands.SellingPrices;
using Shared.Application.BaseServices;
using Shared.Domain.Responses;
using Inventory.Domain.Models.DTOS;

namespace Inventory.Application.Services;

public interface ISellingPriceService : IBaseSettingService<SellingPrice, SellingPriceCreateCommand, SellingPriceUpdateCommand>
{
    Task<ApiResponse<IEnumerable<SellingPriceDto>>> GetDtos();
}