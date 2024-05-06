using AAA.ERP.Responses;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Services.BaseServices.interfaces;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;
using AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;
using AAA.ERP.Services.BaseServices.Interfaces;

namespace AAA.ERP.Services.BaseServices.impelemtation;

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

    public override async Task<ApiResponse> Delete(Guid id, bool isValidate = true)
    {
        var validationResult = await _bussinessValidator.ValidateDeleteBussiness(id);
        if (!validationResult.IsValid)
        {
            return new ApiResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                ErrorMessages = validationResult.ListOfErrors
            };
        }
        return await base.Delete(id, isValidate);
    }
}