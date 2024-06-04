using Shared;

namespace Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseUpdateCommand<TResponse> : ICommand<TResponse> 
{
    public Guid Id { get; set; }
    public string? Notes { get; set; }
}