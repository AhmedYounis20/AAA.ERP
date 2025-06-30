using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Inventory;

public class SizeRepository : BaseSettingRepository<Size>, ISizeRepository
{
    public SizeRepository(IApplicationDbContext context) : base(context) { }
}