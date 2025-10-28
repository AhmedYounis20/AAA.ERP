using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using Inventory.Domain.Commands.ManufacturerCompanies;
using Shared.Application.BaseServices;

namespace Inventory.Application.Services;

public interface IManufacturerCompanyService : IBaseSettingService<ManufacturerCompany, ManufacturerCompanyCreateCommand, ManufacturerCompanyUpdateCommand> { }