using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.TreeSetting;

/// <summary>
/// Generic create handler for <see cref="BaseTreeSettingEntity{TEntity}"/>.
/// </summary>
public abstract class BaseTreeSettingCreateHandler<TEntity, TCreate>
    : Setting.BaseSettingCreateHandler<TEntity, TCreate>
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TCreate : BaseTreeSettingCreateCommand<TEntity>
{
    protected BaseTreeSettingCreateHandler(IBaseTreeSettingRepository<TEntity> repository)
        : base(repository)
    {
    }
}
