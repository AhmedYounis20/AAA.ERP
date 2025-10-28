using Inventory.Domain.Models.Entities.PackingUnits;
using Shared.Application.Repositories;

namespace Inventory.Application.Repositories;

public interface IPackingUnitRepository : IBaseSettingRepository<PackingUnit> { }