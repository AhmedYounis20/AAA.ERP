using ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;
using FluentValidation;

namespace ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.CreateCommandValidators;

public class BaseCreateValidator<TCommand, TResponse> : AbstractValidator<TCommand> where TCommand : BaseCreateCommand<TResponse>
{
    public BaseCreateValidator()
    {
        _ = RuleFor(e => e.Notes).MaximumLength(300);
    }
}
