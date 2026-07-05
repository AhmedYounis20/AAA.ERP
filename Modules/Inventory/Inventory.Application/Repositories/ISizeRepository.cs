using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Application.Repositories;

public interface ISizeRepository : IBaseSettingRepository<Size>
{
    Task<string?> GetMaxCodeAsync();
}
