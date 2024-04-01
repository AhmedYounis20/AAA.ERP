using AAA.ERP.Responses;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Services.BaseServices.interfaces;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;
using AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;

namespace AAA.ERP.Services.BaseServices.impelemtation;

public class BaseTreeSettingService<TEntity> : BaseTreeService<TEntity>, IBaseTreeSettingService<TEntity> where TEntity : BaseTreeSettingEntity
{
    private readonly IBaseTreeSettingRepository<TEntity> _repository;
    public BaseTreeSettingService(IBaseTreeSettingRepository<TEntity> repository,IBaseBussinessValidator<TEntity> bussinessValidator) : base(repository, bussinessValidator)
    => _repository = repository;

    public virtual async Task<ApiResponse> SearchByName(string name)
    {
        try
        {
            var entities = await _repository.Search(name);
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }
}