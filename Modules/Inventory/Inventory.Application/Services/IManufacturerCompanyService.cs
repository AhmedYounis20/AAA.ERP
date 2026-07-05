using ERP.Application.Services.BaseServices;
using Inventory.Application.Commands.ManufacturerCompanies;
using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;

namespace Inventory.Application.Services;

public interface IManufacturerCompanyService : IBaseSettingService<ManufacturerCompany, ManufacturerCompanyCreateCommand, ManufacturerCompanyUpdateCommand>;
