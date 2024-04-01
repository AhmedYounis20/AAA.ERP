using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Services.BaseServices.interfaces;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;
using AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;

namespace AAA.ERP.Services.BaseServices.impelemtation;

public class BaseTreeService<TEntity> : BaseService<TEntity>, IBaseTreeService<TEntity> where TEntity : BaseTreeEntity
{
    private readonly IBaseTreeRepository<TEntity> _repository;
    public BaseTreeService(IBaseTreeRepository<TEntity> repository,IBaseBussinessValidator<TEntity> bussinessValidator) : base(repository, bussinessValidator)
    => _repository = repository;
}