using Inventory.Domain.Commands.PackingUnits;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Infrastructure.Services;
public class PackingUnitService :
    BaseSettingService<PackingUnit, PackingUnitCreateCommand, PackingUnitUpdateCommand>, IPackingUnitService
{
    public PackingUnitService(IPackingUnitRepository repository) : base(repository)
    { }
}
