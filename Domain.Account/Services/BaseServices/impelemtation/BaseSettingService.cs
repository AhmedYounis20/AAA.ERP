using System.Net;
using Shared.BaseEntities;
using Shared.BaseServices.interfaces;
using Shared.Responses;

namespace Shared.BaseServices.impelemtation;

public class BaseSettingService<TEntity> : BaseService<TEntity>, IBaseSettingService<TEntity> where TEntity : BaseSettingEntity
{
    private readonly IBaseSettingRepository<TEntity> _repository;
    public BaseSettingService(IBaseSettingRepository<TEntity> repository,IBaseSettingBussinessValidator<TEntity> bussinessValidator) : base(repository, bussinessValidator)
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