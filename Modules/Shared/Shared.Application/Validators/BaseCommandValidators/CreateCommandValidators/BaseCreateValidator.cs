using FluentValidation;
using Shared.Domain.BaseEntities;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;

public abstract class BaseCreateValidator<TCommand, TEntity> : AbstractValidator<TCommand>
    where TCommand : BaseCreateCommand<TEntity>
    where TEntity : BaseEntity
{
    protected BaseCreateValidator()
    {
        // Base validation rules for create operations
    }
}
