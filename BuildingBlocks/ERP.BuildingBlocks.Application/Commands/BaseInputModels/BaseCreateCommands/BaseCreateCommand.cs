using Shared;
using Shared.Responses;

namespace ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;

public class BaseCreateCommand<TResponse> : ICommand<ApiResponse<TResponse>> 
{
    public string? Notes { get; set; }
}