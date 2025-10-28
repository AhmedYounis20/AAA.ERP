using Inventory.Domain.Models.Entities.SellingPrices;
using Inventory.Domain.Models.DTOS;
using Shared.Application.Repositories;

namespace Inventory.Application.Repositories;

public interface ISellingPriceRepository : IBaseSettingRepository<SellingPrice>
{
    Task<IEnumerable<SellingPriceDto>> GetDtos();
}