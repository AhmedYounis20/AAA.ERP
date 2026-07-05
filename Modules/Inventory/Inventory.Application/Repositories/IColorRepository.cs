using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Colors;

namespace Inventory.Application.Repositories;

public interface IColorRepository : IBaseSettingRepository<Color>
{
    Task<string?> GetMaxCodeAsync();
    Task<bool> GetByColorValueExists(string colorValue);
}
