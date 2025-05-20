using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Inventory;

public class ManufacturerCompanyRepository : BaseSettingRepository<ManufacturerCompany>, IManufacturerCompanyRepository
{
    public ManufacturerCompanyRepository(ApplicationDbContext context) : base(context) { }
}
