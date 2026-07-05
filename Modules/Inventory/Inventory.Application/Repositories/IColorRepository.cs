using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Colors;

namespace Inventory.Application.Repositories;

/// <summary>
/// Repository contract for color persistence.
/// </summary>
public interface IColorRepository : IBaseSettingRepository<Color>
{
    /// <summary>Gets max code async.</summary>
    Task<string?> GetMaxCodeAsync();
    /// <summary>Gets by color value exists.</summary>
    Task<bool> GetByColorValueExists(string colorValue);
}
