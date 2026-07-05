using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Dtos;
using Inventory.Domain.Models.Entities.Items;

namespace Inventory.Application.Repositories
{
    public interface IItemRepository : IBaseTreeSettingRepository<Item>
    {

        Task<IEnumerable<ItemDto>> GetDtos();
        Task<ItemDto?> GetDtoById(Guid id);
        Task<IEnumerable<ItemDto>> GetVariants();
    }
}