using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;

namespace Inventory.Application.Repositories;

public interface ISellingPriceRepository : IBaseSettingRepository<SellingPrice>
{
    Task<IEnumerable<SellingPriceDto>> GetDtos();
}
