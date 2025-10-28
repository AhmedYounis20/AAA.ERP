using Inventory.Domain.Models.Entities.Sizes;
using Shared.Application.Repositories;

namespace Inventory.Application.Repositories;

public interface ISizeRepository : IBaseSettingRepository<Size>
{
    Task<string?> GetMaxCodeAsync();
}