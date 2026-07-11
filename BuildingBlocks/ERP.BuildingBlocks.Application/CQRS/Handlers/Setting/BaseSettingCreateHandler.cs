using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.Setting;

/// <summary>
/// Generic create handler for <see cref="BaseSettingEntity"/>. Mirrors <c>BaseSettingService</c> validation.
/// </summary>
public abstract class BaseSettingCreateHandler<TEntity, TCreate>
    : Base.BaseCreateHandler<TEntity, TCreate>
    where TEntity : BaseSettingEntity
    where TCreate : BaseSettingCreateCommand<TEntity>
{
    protected IBaseSettingRepository<TEntity> SettingRepository =>
        (IBaseSettingRepository<TEntity>)Repository;

    protected BaseSettingCreateHandler(IBaseSettingRepository<TEntity> repository)
        : base(repository)
    {
    }

    protected override async Task<CrudValidationResult> ValidateCreateAsync(
        TCreate command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var existedEntity = await SettingRepository.GetByNames(command.Name, command.NameSecondLanguage);
        if (existedEntity is null)
            return CrudValidationResult.Success;

        var errors = SettingDuplicateNameValidator.CollectDuplicateNameErrors(
            existedEntity,
            command.Name,
            command.NameSecondLanguage);

        return CrudValidationResult.Failure(errors);
    }
}
