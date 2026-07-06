using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.DTOs.Filters;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;
using ERP.Infrastructure.Services.Queries;
using Inventory.Application.Dtos;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Items;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Item read queries.
/// </summary>
public class ItemQueries(
    IItemRepository repository,
    IBaseRepository<ItemCode> codeRepository)
    : BaseTreeSettingRepositoryQueries<Item>(repository),
      IItemQueries
{
    public async Task<ApiResponse<List<ItemDto>>> GetAll()
    {
        var items = await repository.GetDtos();
        return new ApiResponse<List<ItemDto>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items.ToList()
        };
    }

    public async Task<ApiResponse<PaginatedResult<Item>>> GetPaginated(
        ItemFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Expression<Func<Item, bool>>? filterExpression = null;

            if (filter.CreatedFrom.HasValue || filter.CreatedTo.HasValue)
            {
                filterExpression = e =>
                    (!filter.CreatedFrom.HasValue || e.CreatedAt >= filter.CreatedFrom.Value) &&
                    (!filter.CreatedTo.HasValue || e.CreatedAt <= filter.CreatedTo.Value);
            }

            Expression<Func<Item, object>>? orderBy = string.IsNullOrEmpty(filter.SortBy)
                ? e => e.CreatedAt
                : filter.SortBy.ToLower() switch
                {
                    "createdat" => e => e.CreatedAt,
                    "modifiedat" => e => e.ModifiedAt,
                    _ => e => e.CreatedAt
                };

            var result = await repository.GetPaginated(
                filter.PageNumber,
                filter.PageSize,
                filterExpression,
                orderBy,
                filter.SortDescending,
                cancellationToken);

            return new ApiResponse<PaginatedResult<Item>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PaginatedResult<Item>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<ItemDto?>> GetById(Guid id)
    {
        var item = await repository.GetDtoById(id);

        if (item != null && item.NodeType == NodeType.Domain)
        {
            var existingSubDomains = await repository.GetQuery()
                .Where(i => i.ParentId == id && i.NodeType == NodeType.SubDomain)
                .ToListAsync();

            item.SubDomainCombinations = existingSubDomains.Select(subDomain => new ColorSizeCombinationDto
            {
                ColorId = subDomain.ColorId ?? Guid.Empty,
                SizeId = subDomain.SizeId ?? Guid.Empty,
                ApplyDomainChanges = subDomain.ApplyDomainChanges
            }).ToList();
        }

        return new ApiResponse<ItemDto?>
        {
            StatusCode = item == null ? HttpStatusCode.NotFound : HttpStatusCode.OK,
            IsSuccess = item != null,
            Result = item
        };
    }

    public async Task<ApiResponse<string>> GetNextCode(Guid? parentId = null)
    {
        var parent = await codeRepository.GetQuery()
            .Where(e => e.ItemId == parentId && e.CodeType == ItemCodeType.Code)
            .OrderByDescending(e => e.CreatedAt)
            .Select(e => e.Code)
            .FirstOrDefaultAsync();

        var lastSibling = await repository.GetQuery()
            .Where(e => e.ParentId == parentId)
            .OrderByDescending(e => e.CreatedAt)
            .FirstOrDefaultAsync();

        var lastSiblingCode = lastSibling == null
            ? string.Empty
            : await codeRepository.GetQuery()
                .Where(e => e.ItemId == lastSibling.Id && e.CodeType == ItemCodeType.Code)
                .Select(e => e.Code)
                .FirstOrDefaultAsync() ?? string.Empty;

        var code = GetNextChildCode(lastSiblingCode, parent ?? string.Empty);
        return new ApiResponse<string>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = code
        };
    }

    public async Task<ApiResponse<IEnumerable<ItemDto>>> GetVariants()
    {
        var items = await repository.GetVariants();
        return new ApiResponse<IEnumerable<ItemDto>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = items
        };
    }

    private static string GetNextChildCode(string lastChildCode, string parentCode)
    {
        if (string.IsNullOrEmpty(lastChildCode))
            return string.IsNullOrEmpty(parentCode) ? "1" : parentCode + "1";

        var hasParentPrefix = !string.IsNullOrEmpty(parentCode) && lastChildCode.StartsWith(parentCode);
        var suffix = hasParentPrefix
            ? lastChildCode[parentCode.Length..]
            : lastChildCode;

        var match = Regex.Match(suffix, @"^(.*?)(\d+)$");

        string newSuffix;
        if (match.Success)
        {
            var prefix = match.Groups[1].Value;
            var numberPart = match.Groups[2].Value;
            var number = int.Parse(numberPart) + 1;
            var newNumber = number.ToString().PadLeft(numberPart.Length, '0');
            newSuffix = prefix + newNumber;
        }
        else
        {
            newSuffix = suffix + "1";
        }

        return hasParentPrefix ? parentCode + newSuffix : newSuffix;
    }
}
