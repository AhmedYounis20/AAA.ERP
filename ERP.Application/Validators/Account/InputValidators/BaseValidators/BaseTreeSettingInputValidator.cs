using Domain.Account.InputModels.BaseInputModels;

namespace ERP.Application.Validators.Account.InputValidators.BaseValidators;

public class BaseTreeSettingInputValidator<TEntity> : BaseSettingInputValidator<TEntity> where TEntity : BaseTreeSettingInputModel
{
    public BaseTreeSettingInputValidator() : base() { }
}