using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;
using Mapster;
using Shared;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.Base;

/// <summary>
/// Generic update command handler. Behavior mirrors <c>BaseService.Update</c>.
/// </summary>
public abstract class BaseUpdateHandler<TEntity, TUpdate>
    : CrudHandlerExecutionBase<TEntity>,
        ICommandHandler<TUpdate, ApiResponse<TEntity>>
    where TEntity : BaseEntity
    where TUpdate : BaseUpdateCommand<TEntity>
{
    protected BaseUpdateHandler(IBaseRepository<TEntity> repository)
        : base(repository)
    {
    }

    public virtual async Task<ApiResponse<TEntity>> Handle(TUpdate command, CancellationToken cancellationToken)
    {
        try
        {
            TEntity? entity = null;

            if (ShouldRunHooks)
                await BeforeUpdateAsync(command, cancellationToken);

            if (ShouldRunBusinessValidation)
            {
                var validation = await ValidateUpdateAsync(command, cancellationToken);
                if (!validation.IsValid)
                    return CrudResponseFactory.BadRequest<TEntity>(validation.Errors);

                entity = validation.Entity;
            }
            else
            {
                entity = await Repository.Get(command.Id);
            }

            if (entity is null)
                return CrudResponseFactory.Ok<TEntity>(null);

            entity = await MapUpdateEntityAsync(command, entity, cancellationToken);
            await Repository.Update(entity);

            if (ShouldRunHooks)
                await AfterUpdateAsync(entity, command, cancellationToken);

            return await MapUpdateResponseAsync(entity, command, cancellationToken);
        }
        catch (Exception ex)
        {
            return await OnUpdateExceptionAsync(command, ex, cancellationToken)
                   ?? CrudResponseFactory.FromException<TEntity>(ex);
        }
    }

    protected virtual Task BeforeUpdateAsync(TUpdate command, CancellationToken cancellationToken) =>
        Task.CompletedTask;

    protected virtual async Task<CrudEntityValidationResult<TEntity>> ValidateUpdateAsync(
        TUpdate command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await Repository.Get(command.Id);
        if (entity is null)
        {
            return CrudEntityValidationResult<TEntity>.Failure(
                new[] { $"{typeof(TEntity).Name} with Id: {command.Id} not found" });
        }

        return CrudEntityValidationResult<TEntity>.Success(entity);
    }

    protected virtual Task<TEntity> MapUpdateEntityAsync(
        TUpdate command,
        TEntity existingEntity,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var entity = command.Adapt<TEntity>();
        entity.ModifiedAt = DateTime.Now;
        return Task.FromResult(entity);
    }

    protected virtual Task AfterUpdateAsync(
        TEntity entity,
        TUpdate command,
        CancellationToken cancellationToken) => Task.CompletedTask;

    protected virtual Task<ApiResponse<TEntity>> MapUpdateResponseAsync(
        TEntity entity,
        TUpdate command,
        CancellationToken cancellationToken) =>
        Task.FromResult(CrudResponseFactory.Ok(entity));

    protected virtual Task<ApiResponse<TEntity>?> OnUpdateExceptionAsync(
        TUpdate command,
        Exception exception,
        CancellationToken cancellationToken) => Task.FromResult<ApiResponse<TEntity>?>(null);
}
