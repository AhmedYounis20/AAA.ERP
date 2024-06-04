using Domain.Account.Repositories.BaseRepositories.Interfaces;
using Domain.Account.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;
using Microsoft.Extensions.Localization;
using Shared.BaseEntities;
using Shared.Resources;
namespace Domain.Account.Validators.BussinessValidator.BaseBussinessValidators.Impelementation;

public class BaseSettingBussinessValidator<TEntity> : BaseBussinessValidator<TEntity>, IBaseSettingBussinessValidator<TEntity> where TEntity : BaseSettingEntity
{
    private IBaseSettingRepository<TEntity> _repository;
    private IStringLocalizer<Resource> _stringLocalizer;
    public BaseSettingBussinessValidator(IBaseSettingRepository<TEntity> repository, IStringLocalizer<Resource> stringLocalizer) : base(repository)
    {
        _repository = repository;
        _stringLocalizer = stringLocalizer;
    }

    public override async Task<(bool IsValid, List<string> ListOfErrors, TEntity? entity)> ValidateCreateBussiness(TEntity inpuModel)
    {
        bool isValid = true;
        List<string> listOfErrors = new List<string>();
        TEntity? entity = null;

        var existedEntity = await _repository.GetByNames(inpuModel.Name, inpuModel.NameSecondLanguage);
        if (existedEntity != null && existedEntity.Id != inpuModel.Id)
        {
            isValid = false;

            if (existedEntity.Name.Trim().ToUpper() == inpuModel.Name.Trim().ToUpper())
                listOfErrors.Add(_stringLocalizer[typeof(TEntity).Name].Value + " " + _stringLocalizer["WithSameNameIsExisted"].Value);
            if (existedEntity.NameSecondLanguage.Trim().ToUpper() == inpuModel.NameSecondLanguage.Trim().ToUpper())
                listOfErrors.Add(_stringLocalizer[typeof(TEntity).Name].Value + " " + _stringLocalizer["WithSameNameSecondLanguageIsExisted"].Value);
        }

        return (isValid, listOfErrors, entity);
    }

    public override async Task<(bool IsValid, List<string> ListOfErrors, TEntity? entity)> ValidateUpdateBussiness(TEntity inpuModel)
    {
        bool isValid = true;
        List<string> listOfErrors = new List<string>();
        TEntity? entity = null;

        var existedEntity = await _repository.GetByNames(inpuModel.Name, inpuModel.NameSecondLanguage);
        if (existedEntity != null && existedEntity.Id != inpuModel.Id)
        {
            isValid = false;
            if (existedEntity.Name.Trim().ToUpper() == inpuModel.Name.Trim().ToUpper())
                listOfErrors.Add(_stringLocalizer[typeof(TEntity).Name].Value + " " + _stringLocalizer["WithSameNameIsExisted"].Value);
            if (existedEntity.NameSecondLanguage.Trim().ToUpper() == inpuModel.NameSecondLanguage.Trim().ToUpper())
                listOfErrors.Add(_stringLocalizer[typeof(TEntity).Name].Value + " " + _stringLocalizer["WithSameNameSecondLanguageIsExisted"].Value);
        }

        return (isValid, listOfErrors, entity);
    }
}
