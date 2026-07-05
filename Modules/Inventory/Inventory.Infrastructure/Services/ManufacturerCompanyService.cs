using Inventory.Application.Commands.ManufacturerCompanies;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;
using ERP.Infrastructure.Services.BaseServices;

namespace Inventory.Infrastructure.Services;

public class ManufacturerCompanyService
    : BaseSettingService<ManufacturerCompany, ManufacturerCompanyCreateCommand, ManufacturerCompanyUpdateCommand>, IManufacturerCompanyService
{
    public ManufacturerCompanyService(IManufacturerCompanyRepository repository) : base(repository) { }
}
