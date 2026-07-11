using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.PackingUnits;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Packing unit read queries.
/// </summary>
public class PackingUnitQueries(IPackingUnitRepository repository)
    : BaseSettingRepositoryQueries<PackingUnit>(repository),
      IPackingUnitQueries;
