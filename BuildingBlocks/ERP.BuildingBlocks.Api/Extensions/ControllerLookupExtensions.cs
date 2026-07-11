using ERP.BuildingBlocks.Application.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ERP.BuildingBlocks.Api.Extensions;

/// <summary>
/// Standard lookup endpoint helpers for hybrid CQRS controllers.
/// </summary>
public static class ControllerLookupExtensions
{
    public static async Task<IActionResult> ToLookupsResult<TDto>(
        this ControllerBase controller,
        Func<Task<IEnumerable<TDto>>> fetchLookups)
    {
        try
        {
            return controller.ToResult(new ApiResponse<IEnumerable<TDto>>
            {
                IsSuccess = true,
                Result = await fetchLookups()
            });
        }
        catch
        {
            return controller.ToResult(new ApiResponse<IEnumerable<TDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            });
        }
    }
}
