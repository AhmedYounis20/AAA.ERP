using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Inventory.ManufacturerCompanies;
using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;

namespace ERP.Application.Services.Inventory;

public interface IManufacturerCompanyService : IBaseSettingService<ManufacturerCompany, ManufacturerCompanyCreateCommand, ManufacturerCompanyUpdateCommand> { }