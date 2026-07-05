using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.PackingUnits;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Application.Services;

/// <summary>
/// Application service contract for packing unit.
/// </summary>
public interface IPackingUnitService : IBaseSettingService<PackingUnit, PackingUnitCreateCommand, PackingUnitUpdateCommand>;
