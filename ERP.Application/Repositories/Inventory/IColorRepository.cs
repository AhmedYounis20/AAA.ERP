using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Colors;

namespace ERP.Application.Repositories.Inventory;

public interface IColorRepository : IBaseSettingRepository<Color>
{
    Task<string?> GetMaxCodeAsync();
    Task<bool> GetByColorValueExists(string colorValue);
}