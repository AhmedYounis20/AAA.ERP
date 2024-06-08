using System.Net;

namespace Shared.Responses;

public class ApiResponse : ApiResponse<object>;
public class ApiResponse<TResult>
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; } = true;
    public List<string>? ErrorMessages { get; set; }
    public TResult? Result { get; set; }
}
