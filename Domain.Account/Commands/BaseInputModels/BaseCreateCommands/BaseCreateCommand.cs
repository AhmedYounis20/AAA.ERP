using Shared;

namespace Domain.Account.Commands.BaseInputModels.BaseCreateCommands;

public class BaseCreateCommand<TResponse> : ICommand<TResponse> 
{
    public string? Notes { get; set; }
}