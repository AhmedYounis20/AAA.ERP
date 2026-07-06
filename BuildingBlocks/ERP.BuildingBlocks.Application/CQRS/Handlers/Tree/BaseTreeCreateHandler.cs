using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.Tree;

/// <summary>
/// Generic create handler for <see cref="BaseTreeEntity{TEntity}"/>.
/// Reserved for tree-only entities; mirrors <c>BaseTreeService</c> (no extra CRUD logic today).
/// </summary>
public abstract class BaseTreeCreateHandler<TEntity, TCreate>
    : Base.BaseCreateHandler<TEntity, TCreate>
    where TEntity : BaseTreeEntity<TEntity>
    where TCreate : BaseTreeCreateCommand<TEntity>
{
    protected BaseTreeCreateHandler(IBaseTreeRepository<TEntity> repository)
        : base(repository)
    {
    }
}
