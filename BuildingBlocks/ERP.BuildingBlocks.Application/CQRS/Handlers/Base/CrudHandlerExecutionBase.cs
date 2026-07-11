using ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.Base;

/// <summary>
/// Shared repository access and execution flags for all generic CRUD command handlers.
/// </summary>
public abstract class CrudHandlerExecutionBase<TEntity>
    where TEntity : BaseEntity
{
    protected IBaseRepository<TEntity> Repository { get; }

    protected CrudHandlerExecutionBase(IBaseRepository<TEntity> repository)
    {
        Repository = repository;
    }

    /// <summary>
    /// When false, business <c>Validate*Async</c> hooks are skipped (legacy <c>isValidate: false</c>).
    /// </summary>
    protected virtual bool ShouldValidateBusinessRules => true;

    protected CrudExecutionOptions ExecutionOptions { get; set; } = CrudExecutionOptions.Default;

    protected bool ShouldRunHooks => !ExecutionOptions.SuppressHooks;

    protected bool ShouldRunBusinessValidation =>
        ShouldValidateBusinessRules && !ExecutionOptions.SuppressBusinessValidation;
}
