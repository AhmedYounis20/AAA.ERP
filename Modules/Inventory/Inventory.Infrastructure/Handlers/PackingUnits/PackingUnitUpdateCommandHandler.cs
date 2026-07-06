using Inventory.Application.Commands.PackingUnits;
using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.PackingUnits;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

namespace Inventory.Infrastructure.Handlers.PackingUnits;

public sealed class PackingUnitUpdateCommandHandler(IPackingUnitRepository repository)
    : BaseSettingUpdateHandler<PackingUnit, PackingUnitUpdateCommand>(repository);
