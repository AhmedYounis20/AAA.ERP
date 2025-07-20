using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Inventory.Items;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace ERP.Application.Services.Inventory;

public interface IItemService : IBaseTreeSettingService<Item, ItemCreateCommand, ItemUpdateCommand>
{

    Task<ApiResponse<string>> GeNextCode(Guid? parentId = null);
    Task<ApiResponse<List<ItemDto>>> GetItemDtos();
    Task<ApiResponse<ItemDto?>> GetItemDtoById(Guid id);
    Task<ApiResponse<IEnumerable<ItemDto>>> GetVariants();
}