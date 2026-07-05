using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using ERP.BuildingBlocks.Application.Services.Queries;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.Infrastructure.Services.Queries;

/// <summary>
/// Base read queries for setting entities. Transitional: delegates to <see cref="IBaseSettingService{TEntity,TCreateCommand,TUpdateCommand}"/>.
/// </summary>
public class BaseSettingQueries<TEntity, TCreateCommand, TUpdateCommand>(
    IBaseSettingService<TEntity, TCreateCommand, TUpdateCommand> service)
    : ISettingQueries<TEntity>
    where TEntity : BaseSettingEntity
    where TCreateCommand : BaseSettingCreateCommand<TEntity>
    where TUpdateCommand : BaseSettingUpdateCommand<TEntity>
{
    public Task<ApiResponse<IEnumerable<TEntity>>> GetAll()
        => service.ReadAll();

    public Task<ApiResponse<PaginatedResult<TEntity>>> GetPaginated(
        SettingFilterDto filter,
        CancellationToken cancellationToken = default)
        => service.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<TEntity>> GetById(Guid id)
        => service.ReadById(id);

    public Task<ApiResponse<IEnumerable<TEntity>>> SearchByName(string query)
        => service.SearchByName(query);
}
