using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using Domain.Account.Repositories.BaseRepositories.Interfaces;
using Domain.Account.Services.BaseServices.interfaces;
using Domain.Account.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;
using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.Services.BaseServices.impelemtation;

public class BaseTreeService<TEntity, TCreateCommand, TUpdateCommand>
    : BaseService<TEntity, TCreateCommand, TUpdateCommand>, IBaseTreeService<TEntity, TCreateCommand, TUpdateCommand>
    where TEntity : BaseTreeEntity<TEntity>
    where TCreateCommand : BaseTreeCreateCommand<TEntity>
    where TUpdateCommand : BaseTreeUpdateCommand<TEntity>
{
    private readonly IBaseTreeRepository<TEntity> _repository;

    public BaseTreeService(IBaseTreeRepository<TEntity> repository) : base(repository)
        => _repository = repository;
    
    public Task<List<TEntity>> GetChildren(Guid id, int level = 0)
        => _repository.GetChildren(id, level);

    public Task<List<TEntity>> GetLevel(int level = 0)
        => _repository.GetLevel(level);
}