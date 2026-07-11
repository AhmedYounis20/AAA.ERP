using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Application.Services.Queries;

/// <summary>
/// Read-side contract for standard setting entity GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ISettingQueries<TEntity>
    where TEntity : BaseSettingEntity
{
    Task<ApiResponse<IEnumerable<TEntity>>> GetAll();

    Task<ApiResponse<PaginatedResult<TEntity>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<TEntity>> GetById(Guid id);

    Task<ApiResponse<IEnumerable<TEntity>>> SearchByName(string query);
}
