using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.PackingUnits;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Application.Services;

public interface IPackingUnitService : IBaseSettingService<PackingUnit, PackingUnitCreateCommand, PackingUnitUpdateCommand>;
