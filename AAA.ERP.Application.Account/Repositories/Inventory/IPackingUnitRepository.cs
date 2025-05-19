using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;

namespace ERP.Application.Repositories.Inventory;

public interface IPackingUnitRepository : IBaseSettingRepository<PackingUnit> { }