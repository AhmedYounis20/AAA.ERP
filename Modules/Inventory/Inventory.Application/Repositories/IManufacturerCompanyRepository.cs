using Inventory.Domain.Models.Entities.ManufacturerCompanies;
using Shared.Application.Repositories;

namespace Inventory.Application.Repositories
{
    public interface IManufacturerCompanyRepository : IBaseSettingRepository<ManufacturerCompany> { }
}