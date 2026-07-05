using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace Inventory.Application.Repositories;

public interface ISizeRepository : IBaseSettingRepository<Size>
{
    Task<string?> GetMaxCodeAsync();
}
