using System.Text.Json.Serialization;
using Shared;
using Shared.Responses;

namespace Domain.Account.Commands.BaseInputModels.BaseUpdateCommands;

public class BaseUpdateCommand<TResponse> : ICommand<ApiResponse<TResponse>> 
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? Notes { get; set; }
}