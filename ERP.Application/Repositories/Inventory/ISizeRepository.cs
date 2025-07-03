using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace ERP.Application.Repositories.Inventory;

public interface ISizeRepository : IBaseSettingRepository<Size>
{
    Task<string?> GetMaxCodeAsync();
}