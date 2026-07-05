using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Inventory.Domain.Models.Dtos;
using Inventory.Domain.Models.Entities.Items;

namespace Inventory.Application.Repositories
{
    /// <summary>
    /// Repository contract for item persistence.
    /// </summary>
    public interface IItemRepository : IBaseTreeSettingRepository<Item>
    {

        Task<IEnumerable<ItemDto>> GetDtos();
        /// <summary>Gets dto by id.</summary>
        Task<ItemDto?> GetDtoById(Guid id);
        Task<IEnumerable<ItemDto>> GetVariants();
    }
}
