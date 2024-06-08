using Domain.Account.InputModels.BaseInputModels;

namespace AAA.ERP.Validators.InputValidators.BaseValidators;

public class BaseTreeSettingInputValidator<TEntity> : BaseSettingInputValidator<TEntity> where TEntity : BaseTreeSettingInputModel
{
    public BaseTreeSettingInputValidator() :base(){}
}