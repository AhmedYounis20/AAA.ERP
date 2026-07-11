using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;
using System.Net;

namespace Inventory.Infrastructure.Workflows;

/// <summary>
/// Shared validation and delete helpers for catalog workflow services (replaces BaseService inheritance).
/// </summary>
internal static class CatalogWorkflowSupport
{
    public static async Task<(bool isValid, List<string> errors)> ValidateSettingCreateAsync<TEntity, TCreate>(
        IBaseSettingRepository<TEntity> repository,
        TCreate command)
        where TEntity : BaseSettingEntity
        where TCreate : BaseSettingCreateCommand<TEntity>
    {
        var errors = new List<string>();
        var existedEntity = await repository.GetByNames(command.Name, command.NameSecondLanguage);
        if (existedEntity is null)
            return (true, errors);

        if (existedEntity.Name.Trim().ToUpper() == command.Name.Trim().ToUpper())
            errors.Add("WithSameNameIsExisted");
        if (existedEntity.NameSecondLanguage.Trim().ToUpper() == command.NameSecondLanguage.Trim().ToUpper())
            errors.Add("WithSameNameSecondLanguageIsExisted");

        return (errors.Count == 0, errors);
    }

    public static async Task<(bool isValid, List<string> errors, TEntity? entity)> ValidateSettingUpdateAsync<TEntity, TUpdate>(
        IBaseSettingRepository<TEntity> repository,
        TUpdate command)
        where TEntity : BaseSettingEntity
        where TUpdate : BaseSettingUpdateCommand<TEntity>
    {
        var errors = new List<string>();
        var oldEntity = await repository.Get(command.Id);
        var existedEntity = await repository.GetByNames(command.Name, command.NameSecondLanguage);
        if (existedEntity is not null && existedEntity.Id != command.Id)
        {
            if (existedEntity.Name.Trim().ToUpper() == command.Name.Trim().ToUpper())
                errors.Add("WithSameNameIsExisted");
            if (existedEntity.NameSecondLanguage.Trim().ToUpper() == command.NameSecondLanguage.Trim().ToUpper())
                errors.Add("WithSameNameSecondLanguageIsExisted");
        }

        return (errors.Count == 0, errors, oldEntity);
    }

    public static async Task<(bool isValid, List<string> errors, TEntity? entity)> ValidateTreeSettingUpdateAsync<TEntity, TUpdate>(
        IBaseTreeSettingRepository<TEntity> repository,
        TUpdate command)
        where TEntity : BaseTreeSettingEntity<TEntity>
        where TUpdate : BaseTreeSettingUpdateCommand<TEntity>
    {
        var result = await ValidateSettingUpdateAsync(repository, command);
        if (result.entity?.NodeType != command.NodeType)
        {
            result.errors.Add(ErrorMessages.NodeTypeCannotBeChanged);
            result.isValid = false;
        }

        return result;
    }

    public static async Task<(bool isValid, List<string> errors, TEntity? entity)> ValidateTreeSettingDeleteAsync<TEntity>(
        IBaseTreeSettingRepository<TEntity> repository,
        Guid id)
        where TEntity : BaseTreeSettingEntity<TEntity>
    {
        if (await repository.HasChildren(id))
            return (false, ["CannotDeleteParent"], null);

        return await ValidateEntityDeleteAsync(repository, id);
    }

    public static async Task<(bool isValid, List<string> errors, TEntity? entity)> ValidateEntityUpdateAsync<TEntity, TUpdate>(
        IBaseRepository<TEntity> repository,
        TUpdate command)
        where TEntity : BaseEntity
        where TUpdate : BaseUpdateCommand<TEntity>
    {
        var entity = await repository.Get(command.Id);
        if (entity is null)
            return (false, [$"{typeof(TEntity).Name} with Id: {command.Id} not found"], null);

        return (true, [], entity);
    }

    public static async Task<(bool isValid, List<string> errors, TEntity? entity)> ValidateEntityDeleteAsync<TEntity>(
        IBaseRepository<TEntity> repository,
        Guid id)
        where TEntity : BaseEntity
    {
        var entity = await repository.Get(id);
        if (entity is null)
            return (false, [ErrorMessages.RecordNotFound], null);

        return (true, [], entity);
    }

    public static async Task<ApiResponse<TEntity>> DeleteByIdAsync<TEntity>(
        IBaseRepository<TEntity> repository,
        Guid id,
        Func<Guid, Task<(bool isValid, List<string> errors, TEntity? entity)>> validateDelete)
        where TEntity : BaseEntity
    {
        var validationResult = await validateDelete(id);
        if (!validationResult.isValid)
        {
            return new ApiResponse<TEntity>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = validationResult.errors.Select(e => new MessageTemplate { MessageKey = e }).ToList()
            };
        }

        if (validationResult.entity is not null)
            await repository.Delete(validationResult.entity);

        return new ApiResponse<TEntity>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = validationResult.entity
        };
    }
}
