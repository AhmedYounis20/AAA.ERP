using ERP.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace ERP.Application.Repositories.Inventory
{
    public interface IItemRepository : IBaseTreeSettingRepository<Item> { }
}