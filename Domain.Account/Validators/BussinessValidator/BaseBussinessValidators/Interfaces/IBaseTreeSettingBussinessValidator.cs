using Shared.BaseEntities;

namespace Domain.Account.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;

public interface IBaseTreeSettingBussinessValidator<TEntity> : IBaseSettingBussinessValidator<TEntity> where TEntity : BaseTreeSettingEntity<TEntity>
{

    public Task<(bool IsValid, List<string> ListOfErrors, TEntity? entity)> ValidateDeleteBussiness(Guid id);

}