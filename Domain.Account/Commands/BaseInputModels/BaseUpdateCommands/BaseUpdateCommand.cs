using Shared;
using Shared.Responses;

namespace Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseUpdateCommand<TResponse> : ICommand<ApiResponse<TResponse>> 
{
    public Guid Id { get; set; }
    public string? Notes { get; set; }
}