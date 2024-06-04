using Shared;

namespace Domain.Account.Commands.BaseInputModels.BaseCreateCommands;

public class BaseInputModel<TCommand> : ICommand<TCommand> 
{
    public string? Notes { get; set; }
}