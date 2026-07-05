using Inventory.Application.Repositories;
using Inventory.Application.Commands.PackingUnits;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;
using ERP.Infrastructure.Services.BaseServices;

namespace Inventory.Infrastructure.Services;

public class PackingUnitService
    : BaseSettingService<PackingUnit, PackingUnitCreateCommand, PackingUnitUpdateCommand>, IPackingUnitService
{
    public PackingUnitService(IPackingUnitRepository repository) : base(repository) { }
}
