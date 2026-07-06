using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;
using Shared;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.Base;

/// <summary>
/// Generic delete command handler. Behavior mirrors <c>BaseService.Delete(Guid)</c>.
/// </summary>
public abstract class BaseDeleteHandler<TEntity, TDeleteCommand>
    : CrudHandlerExecutionBase<TEntity>,
        ICommandHandler<TDeleteCommand, ApiResponse<TEntity>>
    where TEntity : BaseEntity
    where TDeleteCommand : ICommand<ApiResponse<TEntity>>, IEntityDeleteCommand
{
    protected BaseDeleteHandler(IBaseRepository<TEntity> repository)
        : base(repository)
    {
    }

    public virtual async Task<ApiResponse<TEntity>> Handle(
        TDeleteCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            if (ShouldRunHooks)
                await BeforeDeleteAsync(command, cancellationToken);

            TEntity? entity = null;

            if (ShouldRunBusinessValidation)
            {
                var validation = await ValidateDeleteAsync(command.Id, cancellationToken);
                if (!validation.IsValid)
                    return CrudResponseFactory.BadRequest<TEntity>(validation.Errors);

                entity = validation.Entity;
            }
            else
            {
                entity = await Repository.Get(command.Id);
            }

            if (entity is not null)
                await Repository.Delete(entity);

            if (ShouldRunHooks)
                await AfterDeleteAsync(entity, command, cancellationToken);

            return await MapDeleteResponseAsync(entity, command, cancellationToken);
        }
        catch (Exception ex)
        {
            return await OnDeleteExceptionAsync(command, ex, cancellationToken)
                   ?? CrudResponseFactory.FromException<TEntity>(ex);
        }
    }

    protected virtual Task BeforeDeleteAsync(TDeleteCommand command, CancellationToken cancellationToken) =>
        Task.CompletedTask;

    protected virtual async Task<CrudEntityValidationResult<TEntity>> ValidateDeleteAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await Repository.Get(id);
        if (entity is null)
            return CrudEntityValidationResult<TEntity>.Failure(new[] { ErrorMessages.RecordNotFound });

        return CrudEntityValidationResult<TEntity>.Success(entity);
    }

    protected virtual Task AfterDeleteAsync(
        TEntity? entity,
        TDeleteCommand command,
        CancellationToken cancellationToken) => Task.CompletedTask;

    protected virtual Task<ApiResponse<TEntity>> MapDeleteResponseAsync(
        TEntity? entity,
        TDeleteCommand command,
        CancellationToken cancellationToken) =>
        Task.FromResult(CrudResponseFactory.Ok(entity));

    protected virtual Task<ApiResponse<TEntity>?> OnDeleteExceptionAsync(
        TDeleteCommand command,
        Exception exception,
        CancellationToken cancellationToken) => Task.FromResult<ApiResponse<TEntity>?>(null);
}
