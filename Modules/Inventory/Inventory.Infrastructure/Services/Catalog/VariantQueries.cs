using System.Linq.Expressions;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Dtos;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Variants;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Variant read queries.
/// </summary>
public class VariantQueries(IVariantRepository repository) : IVariantQueries
{
    public async Task<ApiResponse<List<VariantDto>>> GetAll()
    {
        try
        {
            var result = await repository.GetDtos();
            return new ApiResponse<List<VariantDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result.ToList()
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<VariantDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = [ex.Message]
            };
        }
    }

    public async Task<ApiResponse<PaginatedResult<VariantDto>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Expression<Func<Variant, bool>>? filterExpression = null;

            if (filter.CreatedFrom.HasValue || filter.CreatedTo.HasValue)
            {
                filterExpression = e =>
                    (!filter.CreatedFrom.HasValue || e.CreatedAt >= filter.CreatedFrom.Value) &&
                    (!filter.CreatedTo.HasValue || e.CreatedAt <= filter.CreatedTo.Value);
            }

            Expression<Func<Variant, object>>? orderBy = string.IsNullOrEmpty(filter.SortBy)
                ? e => e.CreatedAt
                : filter.SortBy.ToLower() switch
                {
                    "createdat" => e => e.CreatedAt,
                    "modifiedat" => e => e.ModifiedAt,
                    _ => e => e.CreatedAt
                };

            var result = await repository.GetPaginatedDtos(
                filter.PageNumber,
                filter.PageSize,
                filterExpression,
                orderBy,
                filter.SortDescending,
                cancellationToken);

            return new ApiResponse<PaginatedResult<VariantDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PaginatedResult<VariantDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<VariantDto?>> GetById(Guid id)
    {
        try
        {
            var result = await repository.GetDtoById(id);
            return new ApiResponse<VariantDto?>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<VariantDto?>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = [ex.Message]
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<VariantDto>>> GetVariantsByProductId(Guid productId)
    {
        try
        {
            var result = await repository.GetVariantsByProductId(productId);
            return new ApiResponse<IEnumerable<VariantDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<VariantDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = [ex.Message]
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<VariantDto>>> SearchVariants(string searchTerm)
    {
        try
        {
            var result = await repository.SearchVariants(searchTerm);
            return new ApiResponse<IEnumerable<VariantDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<VariantDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessages = [ex.Message]
            };
        }
    }
}
