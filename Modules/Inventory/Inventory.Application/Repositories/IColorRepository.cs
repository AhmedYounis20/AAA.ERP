using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Colors;

namespace Inventory.Application.Repositories;

public interface IColorRepository : IBaseSettingRepository<Color>
{
    Task<string?> GetMaxCodeAsync();
    Task<bool> GetByColorValueExists(string colorValue);
}
