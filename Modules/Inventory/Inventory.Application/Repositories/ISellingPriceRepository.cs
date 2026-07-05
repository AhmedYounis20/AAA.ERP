using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Application.Repositories;

/// <summary>
/// Repository contract for selling price persistence.
/// </summary>
public interface ISellingPriceRepository : IBaseSettingRepository<SellingPrice>
{
    Task<IEnumerable<SellingPriceDto>> GetDtos();
}
