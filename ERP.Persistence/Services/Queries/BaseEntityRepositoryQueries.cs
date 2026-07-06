using System.Linq.Expressions;
using System.Net;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.Infrastructure.Services.Queries;

/// <summary>
/// Read queries for base entities using the repository directly (post-CQRS write migration).
/// </summary>
public class BaseEntityRepositoryQueries<TEntity>(IBaseRepository<TEntity> repository)
    where TEntity : BaseEntity
{
    public async Task<ApiResponse<IEnumerable<TEntity>>> GetAll()
    {
        try
        {
            var entities = await repository.Get();
            return new ApiResponse<IEnumerable<TEntity>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<TEntity>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<MessageTemplate> { new() { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<PaginatedResult<TEntity>>> GetPaginated(
        BaseFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Expression<Func<TEntity, bool>>? filterExpression = null;

            if (filter.CreatedFrom.HasValue || filter.CreatedTo.HasValue)
            {
                filterExpression = e =>
                    (!filter.CreatedFrom.HasValue || e.CreatedAt >= filter.CreatedFrom.Value) &&
                    (!filter.CreatedTo.HasValue || e.CreatedAt <= filter.CreatedTo.Value);
            }

            Expression<Func<TEntity, object>>? orderBy = string.IsNullOrEmpty(filter.SortBy)
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

            return new ApiResponse<PaginatedResult<TEntity>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PaginatedResult<TEntity>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<MessageTemplate> { new() { MessageKey = ex.Message } }
            };
        }
    }

    public async Task<ApiResponse<TEntity>> GetById(Guid id)
    {
        try
        {
            var entity = await repository.Get(id);
            return new ApiResponse<TEntity>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<TEntity>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = new List<MessageTemplate> { new() { MessageKey = ex.Message } }
            };
        }
    }
}
