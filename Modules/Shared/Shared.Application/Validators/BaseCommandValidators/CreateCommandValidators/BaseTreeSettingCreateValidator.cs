using FluentValidation;
using Shared.Domain.BaseEntities;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;

public abstract class BaseTreeSettingCreateValidator<TCommand, TEntity> : BaseSettingCreateValidator<TCommand, TEntity>
    where TCommand : BaseTreeSettingCreateCommand<TEntity>
    where TEntity : BaseTreeSettingEntity<TEntity>
{
    protected BaseTreeSettingCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.NameSecondLanguage).NotEmpty().WithMessage("NameSecondLanguage is required");
    }
}
