using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;
using Mapster;
using Shared;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.Base;

/// <summary>
/// Generic create command handler. Behavior mirrors <c>BaseService.Create</c>.
/// </summary>
public abstract class BaseCreateHandler<TEntity, TCreate>
    : CrudHandlerExecutionBase<TEntity>,
        ICommandHandler<TCreate, ApiResponse<TEntity>>
    where TEntity : BaseEntity
    where TCreate : BaseCreateCommand<TEntity>
{
    protected BaseCreateHandler(IBaseRepository<TEntity> repository)
        : base(repository)
    {
    }

    public virtual async Task<ApiResponse<TEntity>> Handle(TCreate command, CancellationToken cancellationToken)
    {
        try
        {
            if (ShouldRunHooks)
                await BeforeCreateAsync(command, cancellationToken);

            if (ShouldRunBusinessValidation)
            {
                var validation = await ValidateCreateAsync(command, cancellationToken);
                if (!validation.IsValid)
                    return CrudResponseFactory.BadRequest<TEntity>(validation.Errors);
            }

            var entity = await MapCreateEntityAsync(command, cancellationToken);
            await Repository.Add(entity);

            if (ShouldRunHooks)
                await AfterCreateAsync(entity, command, cancellationToken);

            return await MapCreateResponseAsync(entity, command, cancellationToken);
        }
        catch (Exception ex)
        {
            return await OnCreateExceptionAsync(command, ex, cancellationToken)
                   ?? CrudResponseFactory.FromException<TEntity>(ex);
        }
    }

    protected virtual Task BeforeCreateAsync(TCreate command, CancellationToken cancellationToken) =>
        Task.CompletedTask;

    protected virtual Task<CrudValidationResult> ValidateCreateAsync(
        TCreate command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(CrudValidationResult.Success);
    }

    protected virtual Task<TEntity> MapCreateEntityAsync(TCreate command, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(command.Adapt<TEntity>());
    }

    protected virtual Task AfterCreateAsync(
        TEntity entity,
        TCreate command,
        CancellationToken cancellationToken) => Task.CompletedTask;

    protected virtual Task<ApiResponse<TEntity>> MapCreateResponseAsync(
        TEntity entity,
        TCreate command,
        CancellationToken cancellationToken) =>
        Task.FromResult(CrudResponseFactory.Created(entity));

    protected virtual Task<ApiResponse<TEntity>?> OnCreateExceptionAsync(
        TCreate command,
        Exception exception,
        CancellationToken cancellationToken) => Task.FromResult<ApiResponse<TEntity>?>(null);
}
