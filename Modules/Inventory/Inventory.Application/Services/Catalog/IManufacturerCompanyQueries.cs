using ERP.BuildingBlocks.Application.Services.Queries;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for manufacturer company GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IManufacturerCompanyQueries : ISettingQueries<ManufacturerCompany>
{
}
