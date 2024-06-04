using Domain.Account.Repositories.BaseRepositories.Interfaces;
using Domain.Account.Services.BaseServices.interfaces;
using Domain.Account.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;
using Shared.BaseEntities;

namespace Domain.Account.Services.BaseServices.impelemtation;

public class BaseTreeService<TEntity> : BaseService<TEntity>, IBaseTreeService<TEntity> where TEntity : BaseTreeEntity<TEntity>
{
    private readonly IBaseTreeRepository<TEntity> _repository;
    public BaseTreeService(IBaseTreeRepository<TEntity> repository, IBaseBussinessValidator<TEntity> bussinessValidator) : base(repository, bussinessValidator)
    => _repository = repository;

    public Task<List<TEntity>> GetChildren(Guid id, int level = 0)
    => _repository.GetChildren(id, level);

    public Task<List<TEntity>> GetLevel(int level = 0)
    => _repository.GetLevel(level);
}