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
    IBaseTreeSettingRepository<TEntity> _repository;

    public BaseTreeSettingBussinessValidator(IBaseTreeSettingRepository<TEntity> repository, IStringLocalizer<Resource> stringLocalizer) : base(repository, stringLocalizer)
        => _repository = repository;

    public async Task<(bool IsValid, List<string> ListOfErrors, TEntity? entity)> ValidateDeleteBussiness(Guid id)
    {
        bool IsParent = await _repository.HasChildren(id);
        if (IsParent)
        {
            return (false, new List<string> { "CannotDeleteParent" }, null);
        }

        return (true, new List<string> { },null);
    }
}