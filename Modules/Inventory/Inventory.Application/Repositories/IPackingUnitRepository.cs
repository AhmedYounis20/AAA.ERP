using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Application.Repositories;

public interface IPackingUnitRepository : IBaseSettingRepository<PackingUnit>;
