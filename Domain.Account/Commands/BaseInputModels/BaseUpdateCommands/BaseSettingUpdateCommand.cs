using Shared;

namespace Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseUpdateCommand<TCommand> : ICommand<TCommand> 
{
    public Guid Id { get; set; }
    public string? Notes { get; set; }
}