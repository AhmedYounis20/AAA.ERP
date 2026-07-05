using System.Text.Json.Serialization;
using Shared;
using Shared.Responses;

namespace ERP.BuildingBlocks.Application.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseUpdateCommand<TResponse> : ICommand<ApiResponse<TResponse>>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? Notes { get; set; }
}