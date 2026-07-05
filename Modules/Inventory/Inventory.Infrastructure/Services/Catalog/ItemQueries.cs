using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.DTOs.Filters;
using Inventory.Application.Dtos;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Items;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Item read queries. Transitional: delegates to <see cref="IItemService"/> until write methods are removed.
/// </summary>
public class ItemQueries(IItemService itemService) : IItemQueries
{
    public Task<ApiResponse<List<ItemDto>>> GetAll()
        => itemService.GetItemDtos();

    public Task<ApiResponse<PaginatedResult<Item>>> GetPaginated(
        ItemFilterDto filter,
        CancellationToken cancellationToken = default)
        => itemService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<ItemDto?>> GetById(Guid id)
        => itemService.GetItemDtoById(id);

    public Task<ApiResponse<string>> GetNextCode(Guid? parentId = null)
        => itemService.GeNextCode(parentId);

    public Task<ApiResponse<IEnumerable<ItemDto>>> GetVariants()
        => itemService.GetVariants();

    public async Task<ApiResponse<IEnumerable<Item>>> GetLevel(int level = 0)
    {
        var items = await itemService.GetLevel(level);
        return new ApiResponse<IEnumerable<Item>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }

    public async Task<ApiResponse<IEnumerable<Item>>> GetChildren(Guid parentId, int level = 0)
    {
        var items = await itemService.GetChildren(parentId, level);
        return new ApiResponse<IEnumerable<Item>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }

    public Task<ApiResponse<IEnumerable<Item>>> SearchByName(string name)
        => itemService.SearchByName(name);
}
