using Shared.BaseEntities;

namespace Domain.Account.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;

public interface IBaseSettingBussinessValidator<TEntity> : IBaseBussinessValidator<TEntity> where TEntity : BaseSettingEntity
{ }