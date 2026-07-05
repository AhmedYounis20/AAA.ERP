using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace Inventory.Infrastructure.Repositories;

public class PackingUnitRepository : BaseSettingRepository<PackingUnit>, IPackingUnitRepository
{
    public PackingUnitRepository(IApplicationDbContext context) : base(context) { }
}
