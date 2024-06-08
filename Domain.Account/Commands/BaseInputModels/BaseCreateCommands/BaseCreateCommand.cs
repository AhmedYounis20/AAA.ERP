using Shared;
using Shared.Responses;

namespace Domain.Account.Commands.BaseInputModels.BaseCreateCommands;

public class BaseCreateCommand<TResponse> : ICommand<ApiResponse<TResponse>> 
{
    public string? Notes { get; set; }
}