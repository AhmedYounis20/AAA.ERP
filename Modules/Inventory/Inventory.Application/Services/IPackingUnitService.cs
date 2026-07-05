using ERP.Application.Services.BaseServices;
using Inventory.Application.Commands.PackingUnits;
using ERP.Domain.Models.Entities.Inventory.PackingUnits;

namespace Inventory.Application.Services;

public interface IPackingUnitService : IBaseSettingService<PackingUnit, PackingUnitCreateCommand, PackingUnitUpdateCommand>;
