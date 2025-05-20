using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.ManufacturerCompanies;
using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;

namespace ERP.Infrastracture.Services.Inventory;
public class ManufacturerCompanyService :
    BaseSettingService<ManufacturerCompany, ManufacturerCompanyCreateCommand, ManufacturerCompanyUpdateCommand>, IManufacturerCompanyService
{
    public ManufacturerCompanyService(IManufacturerCompanyRepository repository) : base(repository)
    { }
}