using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Application.Repositories;

/// <summary>
/// Repository contract for packing unit persistence.
/// </summary>
public interface IPackingUnitRepository : IBaseSettingRepository<PackingUnit>;
