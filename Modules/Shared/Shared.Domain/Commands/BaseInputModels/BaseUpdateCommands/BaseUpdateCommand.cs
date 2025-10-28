using Shared.Domain.CQRS;
using Shared.Domain.Responses;
using System.Text.Json.Serialization;

namespace Shared.Domain.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseUpdateCommand<TResponse> : ICommand<ApiResponse<TResponse>> 
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? Notes { get; set; }
}

public class BaseUpdateCommand: ICommand<ApiResponse>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? Notes { get; set; }
}