using Inventory.Application.Repositories;
using ERP.Domain.Models.Entities.Inventory.ManufacturerCompanies;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace Inventory.Infrastructure.Repositories;

public class ManufacturerCompanyRepository : BaseSettingRepository<ManufacturerCompany>, IManufacturerCompanyRepository
{
    public ManufacturerCompanyRepository(IApplicationDbContext context) : base(context) { }
}
