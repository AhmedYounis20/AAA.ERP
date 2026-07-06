using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Domain.Entities;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.Tree;

/// <summary>
/// Generic update handler for <see cref="BaseTreeEntity{TEntity}"/>.
/// </summary>
public abstract class BaseTreeUpdateHandler<TEntity, TUpdate>
    : Base.BaseUpdateHandler<TEntity, TUpdate>
    where TEntity : BaseTreeEntity<TEntity>
    where TUpdate : BaseTreeUpdateCommand<TEntity>
{
    protected BaseTreeUpdateHandler(IBaseTreeRepository<TEntity> repository)
        : base(repository)
    {
    }
}
