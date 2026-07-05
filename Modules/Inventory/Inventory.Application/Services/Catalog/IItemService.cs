using Inventory.Application.Repositories;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Inventory.Application.Commands.Items;
using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.Items;

namespace Inventory.Application.Services;

/// <summary>
/// Application service contract for item.
/// </summary>
public interface IItemService : IBaseTreeSettingService<Item, ItemCreateCommand, ItemUpdateCommand>
{
    Task<ApiResponse<string>> GeNextCode(Guid? parentId = null);
    Task<ApiResponse<List<ItemDto>>> GetItemDtos();
    Task<ApiResponse<ItemDto?>> GetItemDtoById(Guid id);
    Task<ApiResponse<IEnumerable<ItemDto>>> GetVariants();
}
