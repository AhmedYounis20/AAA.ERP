using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.PackingUnits;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;

namespace ERP.Infrastracture.Services.Inventory;
public class PackingUnitService :
    BaseSettingService<PackingUnit, PackingUnitCreateCommand, PackingUnitUpdateCommand>, IPackingUnitService
{
    public PackingUnitService(IPackingUnitRepository repository) : base(repository)
    { }
}