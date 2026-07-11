using System.Linq.Expressions;
using System.Net;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.Queries;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Persistence.Services.Queries;

/// <summary>
/// Read queries for tree setting entities using the repository directly (post-CQRS write migration).
/// </summary>
public class BaseTreeSettingRepositoryQueries<TEntity>(IBaseTreeSettingRepository<TEntity> repository)
    : BaseSettingRepositoryQueries<TEntity>(repository),
      ITreeSettingQueries<TEntity>
    where TEntity : BaseTreeSettingEntity<TEntity>
{
    public async Task<ApiResponse<IEnumerable<TEntity>>> GetLevel(int level = 0)
    {
        try
        {
            var items = await repository.GetLevel(level);
            return new ApiResponse<IEnumerable<TEntity>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = items
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

    public async Task<ApiResponse<IEnumerable<TEntity>>> GetChildren(Guid parentId, int level = 0)
    {
        try
        {
            var items = await repository.GetChildren(parentId, level);
            return new ApiResponse<IEnumerable<TEntity>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = items
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
}
