using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Application.Services.Queries;

/// <summary>
/// Read-side contract for hierarchical setting entity GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ITreeSettingQueries<TEntity> : ISettingQueries<TEntity>
    where TEntity : BaseTreeSettingEntity<TEntity>
{
    Task<ApiResponse<IEnumerable<TEntity>>> GetLevel(int level = 0);

    Task<ApiResponse<IEnumerable<TEntity>>> GetChildren(Guid parentId, int level = 0);
}
