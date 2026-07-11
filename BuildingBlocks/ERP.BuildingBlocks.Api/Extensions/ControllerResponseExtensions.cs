using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ERP.BuildingBlocks.Api.Extensions;

/// <summary>
/// Maps <see cref="ApiResponse{T}"/> to MVC <see cref="IActionResult"/> with standard success messages.
/// </summary>
public static class ControllerResponseExtensions
{
    /// <summary>
    /// Returns <see cref="ControllerBase.StatusCode"/> using the response status or an override on success.
    /// </summary>
    public static IActionResult ToResult<T>(
        this ControllerBase controller,
        ApiResponse<T> response,
        HttpStatusCode? successStatus = null,
        string? successMessageKey = null)
    {
        if (response.IsSuccess)
        {
            if (string.IsNullOrEmpty(response.SuccessMessage) && successMessageKey is not null)
                response.Success = new MessageTemplate { MessageKey = successMessageKey };
            else if (response.IsSuccess && string.IsNullOrEmpty(response.SuccessMessage))
                response.Success = new MessageTemplate { MessageKey = SuccessMessages.OperationSuccessful };
        }

        var status = response.IsSuccess && successStatus.HasValue
            ? successStatus.Value
            : response.StatusCode;

        return controller.StatusCode((int)status, response);
    }
}
