using FluentValidation;
using Shared.Domain.BaseEntities;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;

public abstract class BaseTreeCreateValidator<TCommand, TEntity> : BaseCreateValidator<TCommand, TEntity>
    where TCommand : BaseTreeCreateCommand<TEntity>
    where TEntity : BaseTreeEntity<TEntity>
{
    protected BaseTreeCreateValidator()
    {
        // Tree-specific validation rules can be added here
    }
}
