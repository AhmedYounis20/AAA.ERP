using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.DTOs.Filters;
using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.Items;
using ERP.BuildingBlocks.Application.Responses;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for item GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IItemQueries
{
    Task<ApiResponse<List<ItemDto>>> GetAll();

    Task<ApiResponse<PaginatedResult<Item>>> GetPaginated(
        ItemFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<ItemDto?>> GetById(Guid id);

    Task<ApiResponse<string>> GetNextCode(Guid? parentId = null);

    Task<ApiResponse<IEnumerable<ItemDto>>> GetVariants();

    Task<ApiResponse<IEnumerable<Item>>> GetLevel(int level = 0);

    Task<ApiResponse<IEnumerable<Item>>> GetChildren(Guid parentId, int level = 0);

    Task<ApiResponse<IEnumerable<Item>>> SearchByName(string name);
}
