using Inventory.Application.Repositories;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;
using ERP.Infrastructure.Repositories.BaseRepositories;

namespace Inventory.Infrastructure.Repositories;

public class PackingUnitRepository : BaseSettingRepository<PackingUnit>, IPackingUnitRepository
{
    public PackingUnitRepository(IApplicationDbContext context) : base(context) { }
}
