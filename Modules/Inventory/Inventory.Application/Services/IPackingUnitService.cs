using Inventory.Domain.Models.Entities.PackingUnits;
using Inventory.Domain.Commands.PackingUnits;
using Shared.Application.BaseServices;

namespace Inventory.Application.Services;

public interface IPackingUnitService : IBaseSettingService<PackingUnit, PackingUnitCreateCommand, PackingUnitUpdateCommand> { }
