using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;
using Shared;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.TreeSetting;

/// <summary>
/// Generic delete handler for <see cref="BaseTreeSettingEntity{TEntity}"/>.
/// Mirrors <c>BaseTreeSettingService.ValidateDelete</c> and delete guard.
/// </summary>
public abstract class BaseTreeSettingDeleteHandler<TEntity, TDeleteCommand>
    : Setting.BaseSettingDeleteHandler<TEntity, TDeleteCommand>
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TDeleteCommand : ICommand<ApiResponse<TEntity>>, IEntityDeleteCommand
{
    protected IBaseTreeSettingRepository<TEntity> TreeSettingRepository =>
        (IBaseTreeSettingRepository<TEntity>)Repository;

    protected BaseTreeSettingDeleteHandler(IBaseTreeSettingRepository<TEntity> repository)
        : base(repository)
    {
    }

    protected override async Task<CrudEntityValidationResult<TEntity>> ValidateDeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (await TreeSettingRepository.HasChildren(id))
            return CrudEntityValidationResult<TEntity>.Failure(new[] { "CannotDeleteParent" });

        return await base.ValidateDeleteAsync(id, cancellationToken);
    }
}
