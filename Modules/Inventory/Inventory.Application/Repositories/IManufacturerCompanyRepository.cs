using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;

namespace Inventory.Application.Repositories;

public interface IManufacturerCompanyRepository : IBaseSettingRepository<ManufacturerCompany>;
