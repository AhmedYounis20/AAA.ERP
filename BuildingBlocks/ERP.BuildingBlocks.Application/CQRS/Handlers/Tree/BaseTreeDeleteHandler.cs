using ERP.BuildingBlocks.Application.CQRS.Commands;
using ERP.BuildingBlocks.Application.CQRS.Handlers.Base;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;
using Shared;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.Tree;

/// <summary>
/// Generic delete handler for <see cref="BaseTreeEntity{TEntity}"/>.
/// </summary>
public abstract class BaseTreeDeleteHandler<TEntity, TDeleteCommand>
    : BaseDeleteHandler<TEntity, TDeleteCommand>
    where TEntity : BaseTreeEntity<TEntity>
    where TDeleteCommand : ICommand<ApiResponse<TEntity>>, IEntityDeleteCommand
{
    protected BaseTreeDeleteHandler(IBaseTreeRepository<TEntity> repository)
        : base(repository)
    {
    }
}
