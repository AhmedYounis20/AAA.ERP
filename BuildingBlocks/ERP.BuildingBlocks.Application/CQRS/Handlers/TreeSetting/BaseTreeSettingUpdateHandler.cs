using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.TreeSetting;

/// <summary>
/// Generic update handler for <see cref="BaseTreeSettingEntity{TEntity}"/>.
/// Mirrors <c>BaseTreeSettingService.ValidateUpdate</c>.
/// </summary>
public abstract class BaseTreeSettingUpdateHandler<TEntity, TUpdate>
    : Setting.BaseSettingUpdateHandler<TEntity, TUpdate>
    where TEntity : BaseTreeSettingEntity<TEntity>
    where TUpdate : BaseTreeSettingUpdateCommand<TEntity>
{
    protected IBaseTreeSettingRepository<TEntity> TreeSettingRepository =>
        (IBaseTreeSettingRepository<TEntity>)Repository;

    protected BaseTreeSettingUpdateHandler(IBaseTreeSettingRepository<TEntity> repository)
        : base(repository)
    {
    }

    protected override async Task<CrudEntityValidationResult<TEntity>> ValidateUpdateAsync(
        TUpdate command,
        CancellationToken cancellationToken)
    {
        var result = await base.ValidateUpdateAsync(command, cancellationToken);

        if (!result.IsValid)
            return result;

        if (result.Entity?.NodeType != command.NodeType)
        {
            var errors = result.Errors.ToList();
            errors.Add(ErrorMessages.NodeTypeCannotBeChanged);
            return CrudEntityValidationResult<TEntity>.Failure(errors, result.Entity);
        }

        return result;
    }
}
