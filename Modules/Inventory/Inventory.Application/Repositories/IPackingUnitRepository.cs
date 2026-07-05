using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;

namespace Inventory.Application.Repositories;

public interface IPackingUnitRepository : IBaseSettingRepository<PackingUnit>;
