using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

/// <summary>
/// Generic update handler for <see cref="BaseSettingEntity"/>. Mirrors <c>BaseSettingService</c> validation.
/// </summary>
public abstract class BaseSettingUpdateHandler<TEntity, TUpdate>
    : Base.BaseUpdateHandler<TEntity, TUpdate>
    where TEntity : BaseSettingEntity
    where TUpdate : BaseSettingUpdateCommand<TEntity>
{
    protected IBaseSettingRepository<TEntity> SettingRepository =>
        (IBaseSettingRepository<TEntity>)Repository;

    protected BaseSettingUpdateHandler(IBaseSettingRepository<TEntity> repository)
        : base(repository)
    {
    }

    protected override async Task<CrudEntityValidationResult<TEntity>> ValidateUpdateAsync(
        TUpdate command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var oldEntity = await SettingRepository.Get(command.Id);
        var existedEntity = await SettingRepository.GetByNames(command.Name, command.NameSecondLanguage);

        if (existedEntity is not null && existedEntity.Id != command.Id)
        {
            var errors = SettingDuplicateNameValidator.CollectDuplicateNameErrors(
                existedEntity,
                command.Name,
                command.NameSecondLanguage);

            return CrudEntityValidationResult<TEntity>.Failure(errors, oldEntity);
        }

        return CrudEntityValidationResult<TEntity>.Success(oldEntity!);
    }
}
