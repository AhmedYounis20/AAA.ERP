using FluentValidation;
using Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

namespace Account.Application.Validators.CommandValidators.BaseCommandValidators.CreateCommandValidators;

public class BaseCreateValidator<TCommand, TResponse> : AbstractValidator<TCommand> where TCommand : BaseCreateCommand<TResponse>
{
    public BaseCreateValidator()
    {
        _ = RuleFor(e => e.Notes).MaximumLength(300);
    }
}
