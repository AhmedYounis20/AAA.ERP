using AAA.ERP.Models.BaseEntities;

namespace AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;

public interface IBaseTreeSettingBussinessValidator<TEntity> : IBaseSettingBussinessValidator<TEntity> where TEntity : BaseTreeSettingEntity<TEntity>
{ }