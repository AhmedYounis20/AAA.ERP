using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.ManufacturerCompanies;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Application.Services;

public interface IManufacturerCompanyService : IBaseSettingService<ManufacturerCompany, ManufacturerCompanyCreateCommand, ManufacturerCompanyUpdateCommand>;
