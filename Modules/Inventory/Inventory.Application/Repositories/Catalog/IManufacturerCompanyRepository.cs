using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Application.Repositories;

/// <summary>
/// Repository contract for manufacturer company persistence.
/// </summary>
public interface IManufacturerCompanyRepository : IBaseSettingRepository<ManufacturerCompany>;
