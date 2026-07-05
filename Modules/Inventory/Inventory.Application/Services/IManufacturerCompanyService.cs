using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.ManufacturerCompanies;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Application.Services;

/// <summary>
/// Application service contract for manufacturer company.
/// </summary>
public interface IManufacturerCompanyService : IBaseSettingService<ManufacturerCompany, ManufacturerCompanyCreateCommand, ManufacturerCompanyUpdateCommand>;
