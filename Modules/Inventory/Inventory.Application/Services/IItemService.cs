using Inventory.Domain.Commands.Items;
using Inventory.Domain.Models.DTOS;
using Inventory.Domain.Models.Entities.Items;
using Shared.Application.BaseServices;
using Shared.Domain.Responses;

namespace Inventory.Application.Services;

public interface IItemService : IBaseTreeSettingService<Item, ItemCreateCommand, ItemUpdateCommand>
{

    Task<ApiResponse<string>> GeNextCode(Guid? parentId = null);
    Task<ApiResponse<List<ItemDto>>> GetItemDtos();
    Task<ApiResponse<ItemDto?>> GetItemDtoById(Guid id);
    Task<ApiResponse<IEnumerable<ItemDto>>> GetVariants();
}