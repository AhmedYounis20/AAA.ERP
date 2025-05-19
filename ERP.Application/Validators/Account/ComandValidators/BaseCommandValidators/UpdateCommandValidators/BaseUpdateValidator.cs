using Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;
using FluentValidation;

namespace ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;

public class BaseUpdateValidator<TCommand, TResponse> : AbstractValidator<TCommand> where TCommand : BaseUpdateCommand<TResponse>
{
    public BaseUpdateValidator()
    {
        _ = RuleFor(e => e.Notes).MaximumLength(300);
    }
}
