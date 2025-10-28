using System.Net;
using System.Text.Json.Serialization;

namespace Shared.Responses;

public interface IApiResponse
{
    HttpStatusCode StatusCode { get; set; }
    bool IsSuccess { get; set; }
    string? SuccessMessage { get; set; }
    List<string>? ErrorMessages { get; set; }
    object? Result { get; }
    MessageTemplate? Success { get; set; }
    List<MessageTemplate>? Errors { get; set; }
}

public class ApiResponse : ApiResponse<object>;
public class ApiResponse<TResult> : IApiResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; } = true;
    public string? SuccessMessage { get; set; }
    public List<string>? ErrorMessages { get; set; }
    public TResult? Result { get; set; }

    public MessageTemplate? Success { get; set; }
    public List<MessageTemplate>? Errors { get; set; }

    // Non-generic accessors for filters/middleware
    object? IApiResponse.Result => Result;
}