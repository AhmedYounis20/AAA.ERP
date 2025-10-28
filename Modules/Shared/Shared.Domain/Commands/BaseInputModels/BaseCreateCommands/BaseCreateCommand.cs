using Shared.Domain.CQRS;
using Shared.Domain.Responses;

namespace Shared.Domain.Commands.BaseInputModels.BaseCreateCommands;

public class BaseCreateCommand<TResponse> : ICommand<ApiResponse<TResponse>> 
{
    public string? Notes { get; set; }
}

public class BaseCreateCommand : ICommand<ApiResponse>
{
    public string? Notes { get; set; }
}