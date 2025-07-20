using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;

namespace ERP.Application.Repositories.Inventory;

public interface ISellingPriceRepository : IBaseSettingRepository<SellingPrice>
{
    Task<IEnumerable<SellingPriceDto>> GetDtos();
}