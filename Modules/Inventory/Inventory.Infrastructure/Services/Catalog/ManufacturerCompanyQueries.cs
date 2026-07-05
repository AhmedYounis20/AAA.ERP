using Inventory.Application.Commands.ManufacturerCompanies;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using ERP.Infrastructure.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Manufacturer company read queries. Transitional: delegates to <see cref="IManufacturerCompanyService"/> until write methods are removed.
/// </summary>
public class ManufacturerCompanyQueries(IManufacturerCompanyService manufacturerCompanyService)
    : BaseSettingQueries<ManufacturerCompany, ManufacturerCompanyCreateCommand, ManufacturerCompanyUpdateCommand>(manufacturerCompanyService),
      IManufacturerCompanyQueries
{
}
