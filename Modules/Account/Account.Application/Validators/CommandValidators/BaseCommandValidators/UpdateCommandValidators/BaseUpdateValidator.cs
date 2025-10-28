using FluentValidation;
using Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

namespace Account.Application.Validators.CommandValidators.BaseCommandValidators.UpdateCommandValidators;

public class BaseUpdateValidator<TCommand, TResponse> : AbstractValidator<TCommand> where TCommand : BaseUpdateCommand<TResponse>
{
    public BaseUpdateValidator()
    {
        _ = RuleFor(e => e.Notes).MaximumLength(300);
    }
}
