using ERP.Application.Repositories.Inventory;
using ERP.Domain.Models.Entities.Inventory.Items;
using ERP.Infrastracture.Repositories.BaseRepositories;

namespace ERP.Infrastracture.Repositories.Inventory;

public class ItemRepository : BaseTreeSettingRepository<Item>, IItemRepository
{
    public ItemRepository(IApplicationDbContext context) : base(context) { }
}