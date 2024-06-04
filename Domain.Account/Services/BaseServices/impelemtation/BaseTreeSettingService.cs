using Domain.Account.Repositories.BaseRepositories.Interfaces;
using Domain.Account.Services.BaseServices.interfaces;
using Domain.Account.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;
using Shared.BaseEntities;
using Shared.Responses;

namespace Domain.Account.Services.BaseServices.impelemtation;

public class BaseTreeSettingService<TEntity> : BaseSettingService<TEntity>, IBaseTreeSettingService<TEntity> where TEntity : BaseTreeSettingEntity<TEntity>
{
    private readonly IBaseTreeSettingRepository<TEntity> _repository;
    private readonly IBaseTreeSettingBussinessValidator<TEntity> _bussinessValidator;
    public BaseTreeSettingService(IBaseTreeSettingRepository<TEntity> repository, IBaseTreeSettingBussinessValidator<TEntity> bussinessValidator) : base(repository, bussinessValidator)
    {
        _repository = repository;
        _bussinessValidator = bussinessValidator;
    }
    public Task<List<TEntity>> GetChildren(Guid id, int level = 0)
    => _repository.GetChildren(id, level);

    public Task<List<TEntity>> GetLevel(int level = 0)
    => _repository.GetLevel(level);

    public override async Task<ApiResponse<TEntity>> Delete(Guid id, bool isValidate = true)
    {
        var validationResult = await _bussinessValidator.ValidateDeleteBussiness(id);
        if (!validationResult.IsValid)
        {
            return new ApiResponse<TEntity>
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                ErrorMessages = validationResult.ListOfErrors
            };
        }
        return await base.Delete(id, isValidate);
    }
}