using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Infrastructure.Repositories;

public class PackingUnitRepository : BaseSettingRepository<PackingUnit>, IPackingUnitRepository
{
    public PackingUnitRepository(IInventoryDbContext context) : base(context) { }
}
