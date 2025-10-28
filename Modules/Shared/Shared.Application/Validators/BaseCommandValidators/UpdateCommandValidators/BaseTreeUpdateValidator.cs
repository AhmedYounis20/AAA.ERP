using FluentValidation;
using Shared.Domain.BaseEntities;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

public abstract class BaseTreeUpdateValidator<TCommand, TEntity> : BaseUpdateValidator<TCommand, TEntity>
    where TCommand : BaseTreeUpdateCommand<TEntity>
    where TEntity : BaseTreeEntity<TEntity>
{
    protected BaseTreeUpdateValidator()
    {
        // Tree-specific validation rules can be added here
    }
}
