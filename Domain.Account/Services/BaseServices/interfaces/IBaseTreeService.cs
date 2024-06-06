using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Shared.BaseEntities;

namespace Domain.Account.Services.BaseServices.interfaces;

public interface IBaseTreeService<TEntity, in TCreateCommand,in TUpdateCommand> 
: IBaseService<TEntity,TCreateCommand,TUpdateCommand> 
where TEntity : BaseTreeEntity<TEntity>
where TCreateCommand : BaseTreeCreateCommand<TEntity>
where TUpdateCommand : BaseTreeUpdateCommand<TEntity>
{
    Task<List<TEntity>> GetLevel(int level = 0);
    Task<List<TEntity>> GetChildren(Guid id, int level = 0);
}