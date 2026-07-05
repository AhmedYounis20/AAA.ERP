using Inventory.Application.Commands.PackingUnits;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.PackingUnits;
using ERP.Infrastructure.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Packing unit read queries. Transitional: delegates to <see cref="IPackingUnitService"/> until write methods are removed.
/// </summary>
public class PackingUnitQueries(IPackingUnitService packingUnitService)
    : BaseSettingQueries<PackingUnit, PackingUnitCreateCommand, PackingUnitUpdateCommand>(packingUnitService),
      IPackingUnitQueries
{
}
