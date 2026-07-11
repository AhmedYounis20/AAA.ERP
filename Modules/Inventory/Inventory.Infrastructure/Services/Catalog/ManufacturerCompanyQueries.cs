using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Manufacturer company read queries.
/// </summary>
public class ManufacturerCompanyQueries(IManufacturerCompanyRepository repository)
    : BaseSettingRepositoryQueries<ManufacturerCompany>(repository),
      IManufacturerCompanyQueries;
