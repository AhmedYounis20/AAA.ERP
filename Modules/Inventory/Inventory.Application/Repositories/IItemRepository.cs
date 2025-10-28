using Inventory.Domain.Models.DTOS;
using Inventory.Domain.Models.Entities.Items;
using Shared.Application.Repositories;

namespace Inventory.Application.Repositories
{
    public interface IItemRepository : IBaseTreeSettingRepository<Item>
    {

        Task<IEnumerable<ItemDto>> GetDtos();
        Task<ItemDto?> GetDtoById(Guid id);
        Task<IEnumerable<ItemDto>> GetVariants();
    }
}