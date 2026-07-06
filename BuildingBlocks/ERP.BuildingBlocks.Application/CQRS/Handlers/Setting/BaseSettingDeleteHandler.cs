using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Base;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;
using Shared;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

/// <summary>
/// Generic delete handler for <see cref="BaseSettingEntity"/>.
/// </summary>
public abstract class BaseSettingDeleteHandler<TEntity, TDeleteCommand>
    : BaseDeleteHandler<TEntity, TDeleteCommand>
    where TEntity : BaseSettingEntity
    where TDeleteCommand : ICommand<ApiResponse<TEntity>>, IEntityDeleteCommand
{
    protected BaseSettingDeleteHandler(IBaseSettingRepository<TEntity> repository)
        : base(repository)
    {
    }
}
