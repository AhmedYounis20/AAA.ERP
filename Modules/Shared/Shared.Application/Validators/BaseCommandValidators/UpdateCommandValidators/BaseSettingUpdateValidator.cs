using FluentValidation;
using Shared.Domain.BaseEntities;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

public abstract class BaseSettingUpdateValidator<TCommand, TEntity> : BaseUpdateValidator<TCommand, TEntity>
    where TCommand : BaseSettingUpdateCommand<TEntity>
    where TEntity : BaseSettingEntity
{
    protected BaseSettingUpdateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.NameSecondLanguage).NotEmpty().WithMessage("NameSecondLanguage is required");
    }
}
