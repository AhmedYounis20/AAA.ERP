using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.BaseServices;
using Inventory.Application.Commands.Items;
using ERP.Domain.Models.Dtos.Inventory;
using ERP.Domain.Models.Entities.Inventory.Items;

namespace Inventory.Application.Services;

public interface IItemService : IBaseTreeSettingService<Item, ItemCreateCommand, ItemUpdateCommand>
{
    Task<ApiResponse<string>> GeNextCode(Guid? parentId = null);
    Task<ApiResponse<List<ItemDto>>> GetItemDtos();
    Task<ApiResponse<ItemDto?>> GetItemDtoById(Guid id);
    Task<ApiResponse<IEnumerable<ItemDto>>> GetVariants();
}
