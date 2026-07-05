using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Application.Repositories;

public interface IManufacturerCompanyRepository : IBaseSettingRepository<ManufacturerCompany>;
