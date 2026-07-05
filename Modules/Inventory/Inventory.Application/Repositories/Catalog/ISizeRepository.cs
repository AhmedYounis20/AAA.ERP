using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Application.Repositories;

/// <summary>
/// Repository contract for size persistence.
/// </summary>
public interface ISizeRepository : IBaseSettingRepository<Size>
{
    /// <summary>Gets max code async.</summary>
    Task<string?> GetMaxCodeAsync();
}
