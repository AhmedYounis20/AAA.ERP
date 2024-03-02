using AAA.ERP.Models.BaseEntities;

namespace AAA.ERP.Validators.BussinessValidator.Interfaces;

public interface IBaseSettingBussinessValidator<TEntity> : IBaseBussinessValidator<TEntity> where TEntity : BaseSettingEntity
{ }