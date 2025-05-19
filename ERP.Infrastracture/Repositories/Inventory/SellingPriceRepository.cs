using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Inventory;

public class SellingPriceRepository : BaseSettingRepository<SellingPrice>, ISellingPriceRepository
{
    public SellingPriceRepository(ApplicationDbContext context) : base(context) { }
}
