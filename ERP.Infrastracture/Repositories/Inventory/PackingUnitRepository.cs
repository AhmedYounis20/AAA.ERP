using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Inventory;

public class PackingUnitRepository : BaseSettingRepository<PackingUnit>, IPackingUnitRepository
{
    public PackingUnitRepository(IApplicationDbContext context) : base(context) { }
}