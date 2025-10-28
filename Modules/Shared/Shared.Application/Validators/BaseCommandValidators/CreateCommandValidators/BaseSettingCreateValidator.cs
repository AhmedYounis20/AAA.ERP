using FluentValidation;
using Shared.Domain.BaseEntities;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;

public abstract class BaseSettingCreateValidator<TCommand, TEntity> : BaseCreateValidator<TCommand, TEntity>
    where TCommand : BaseSettingCreateCommand<TEntity>
    where TEntity : BaseSettingEntity
{
    protected BaseSettingCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.NameSecondLanguage).NotEmpty().WithMessage("NameSecondLanguage is required");
    }
}
