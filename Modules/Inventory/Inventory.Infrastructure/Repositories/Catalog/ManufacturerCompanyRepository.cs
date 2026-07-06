using Inventory.Application.Repositories;
using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using ERP.BuildingBlocks.Persistence.Repositories;

namespace Inventory.Infrastructure.Repositories;

public class ManufacturerCompanyRepository : BaseSettingRepository<ManufacturerCompany>, IManufacturerCompanyRepository
{
    public ManufacturerCompanyRepository(IApplicationDbContext context) : base(context) { }
}
