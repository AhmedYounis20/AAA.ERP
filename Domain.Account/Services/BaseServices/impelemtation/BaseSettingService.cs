using Domain.Account.Repositories.BaseRepositories.Interfaces;
using Domain.Account.Services.BaseServices.interfaces;
using Domain.Account.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;
using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.Services.BaseServices.impelemtation;

public class BaseSettingService<TEntity> : BaseService<TEntity>, IBaseSettingService<TEntity> where TEntity : BaseSettingEntity
{
    private readonly IBaseSettingRepository<TEntity> _repository;
    public BaseSettingService(IBaseSettingRepository<TEntity> repository,IBaseSettingBussinessValidator<TEntity> bussinessValidator) : base(repository, bussinessValidator)
    => _repository = repository;

    public virtual async Task<ApiResponse<IEnumerable<TEntity>>> SearchByName(string name)
    {
        try
        {
            var entities = await _repository.Search(name);
            return new ApiResponse<IEnumerable<TEntity>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<TEntity>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }
}