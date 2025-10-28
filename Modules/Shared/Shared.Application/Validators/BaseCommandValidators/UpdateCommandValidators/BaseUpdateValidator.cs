using FluentValidation;
using Shared.Domain.BaseEntities;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

public abstract class BaseUpdateValidator<TCommand, TEntity> : AbstractValidator<TCommand>
    where TCommand : BaseUpdateCommand<TEntity>
    where TEntity : BaseEntity
{
    protected BaseUpdateValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required for update operations");
    }
}
