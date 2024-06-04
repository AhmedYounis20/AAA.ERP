using Domain.Account.Commands.BaseInputModels.BaseCreateCommands;
using FluentValidation;

namespace Domain.Account.Validators.ComandValidators.BaseCommandValidators.CreateCommandValidators;

public class BaseCreateValidator<TCommand,TResponse> : AbstractValidator<TCommand> where TCommand : BaseCreateCommand<TResponse>
{
    public BaseCreateValidator()
    {
        _ = RuleFor(e=>e.Notes).MaximumLength(300);
    }
}
