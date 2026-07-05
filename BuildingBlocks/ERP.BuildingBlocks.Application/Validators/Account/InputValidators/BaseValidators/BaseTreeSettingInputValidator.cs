using ERP.BuildingBlocks.Application.Commands.BaseInputModels;

namespace ERP.BuildingBlocks.Application.Validators.Account.InputValidators.BaseValidators;

public class BaseTreeSettingInputValidator<TEntity> : BaseSettingInputValidator<TEntity> where TEntity : BaseTreeSettingInputModel
{
    public BaseTreeSettingInputValidator() : base() { }
}