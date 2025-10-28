using Inventory.Domain.Models.Entities.Colors;
using Shared.Application.Repositories;

namespace Inventory.Application.Repositories;

public interface IColorRepository : IBaseSettingRepository<Color>
{
    Task<string?> GetMaxCodeAsync();
    Task<bool> GetByColorValueExists(string colorValue);
}