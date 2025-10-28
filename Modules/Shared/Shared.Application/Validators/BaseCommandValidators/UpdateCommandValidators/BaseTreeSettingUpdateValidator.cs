using FluentValidation;
using Shared.Application.Commands.BaseInputModels.BaseUpdateCommands;
using Shared.Domain.BaseEntities;

namespace Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

public abstract class BaseTreeSettingUpdateValidator<TCommand, TEntity> : BaseSettingUpdateValidator<TCommand, TEntity>
    where TCommand : BaseTreeSettingUpdateCommand<TEntity>
    where TEntity : BaseTreeSettingEntity<TEntity>
{
    protected BaseTreeSettingUpdateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.NameSecondLanguage).NotEmpty().WithMessage("NameSecondLanguage is required");
    }
}
