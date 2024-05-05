using AAA.ERP.InputModels.BaseInputModels;
using AAA.ERP.Models.BaseEntities;
using AAA.ERP.Repositories.BaseRepositories.Interfaces;
using AAA.ERP.Resources;
using AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;
using Microsoft.Extensions.Localization;
using System.Xml.Linq;

namespace AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Impelementation;

public class BaseTreeSettingBussinessValidator<TEntity> : BaseSettingBussinessValidator<TEntity>, IBaseTreeSettingBussinessValidator<TEntity> where TEntity : BaseTreeSettingEntity<TEntity>
{

    public BaseTreeSettingBussinessValidator(IBaseSettingRepository<TEntity> repository, IStringLocalizer<Resource> stringLocalizer) : base(repository, stringLocalizer) {}
}