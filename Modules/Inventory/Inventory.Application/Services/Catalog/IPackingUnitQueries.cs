using ERP.BuildingBlocks.Application.Services.Queries;
using Inventory.Domain.Models.Entities.PackingUnits;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for packing unit GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IPackingUnitQueries : ISettingQueries<PackingUnit>
{
}
