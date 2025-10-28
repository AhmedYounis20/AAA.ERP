using Inventory.Domain.Commands.ManufacturerCompanies;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;

namespace Inventory.Infrastructure.Services;
public class ManufacturerCompanyService :
    BaseSettingService<ManufacturerCompany, ManufacturerCompanyCreateCommand, ManufacturerCompanyUpdateCommand>, IManufacturerCompanyService
{
    public ManufacturerCompanyService(IManufacturerCompanyRepository repository) : base(repository)
    { }
}
