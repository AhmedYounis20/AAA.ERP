using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Inventory.PackingUnits;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;

namespace ERP.Application.Services.Inventory;

public interface IPackingUnitService : IBaseSettingService<PackingUnit, PackingUnitCreateCommand, PackingUnitUpdateCommand> { }