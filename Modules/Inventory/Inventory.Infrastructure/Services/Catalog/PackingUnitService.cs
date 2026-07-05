using Inventory.Application.Repositories;
using Inventory.Application.Commands.PackingUnits;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.PackingUnits;
using ERP.Infrastructure.Services.BaseServices;

namespace Inventory.Infrastructure.Services;

public class PackingUnitService
    : BaseSettingService<PackingUnit, PackingUnitCreateCommand, PackingUnitUpdateCommand>, IPackingUnitService
{
    public PackingUnitService(IPackingUnitRepository repository) : base(repository) { }
}
