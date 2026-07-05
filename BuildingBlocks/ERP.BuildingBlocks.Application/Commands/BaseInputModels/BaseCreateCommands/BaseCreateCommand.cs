using Shared;
using ERP.BuildingBlocks.Application.Responses;

namespace ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseCreateCommands;

public class BaseCreateCommand<TResponse> : ICommand<ApiResponse<TResponse>>
{
    public string? Notes { get; set; }
}