using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace Inventory.Application.Repositories
{
    public interface IItemRepository : IBaseTreeSettingRepository<Item>
    {

        Task<IEnumerable<ItemDto>> GetDtos();
        Task<ItemDto?> GetDtoById(Guid id);
        Task<IEnumerable<ItemDto>> GetVariants();
    }
}