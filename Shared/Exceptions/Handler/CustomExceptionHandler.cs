using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Responses;
using System.Net;

namespace Shared.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "Error Message: {ExceptionMessage}, Time of occurrence: {Time}, Path: {Path}",
            exception.Message, DateTime.UtcNow, httpContext.Request.Path);

        var (statusCode, errors) = exception switch
        {
            InternalServerException internalEx => (
                HttpStatusCode.InternalServerError,
                new List<MessageTemplate> { new() { MessageKey = "InternalServerError", Args = new[] { internalEx.Details ?? internalEx.Message } } }
            ),
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                validationEx.Errors.Select(e => new MessageTemplate { MessageKey = e.ErrorMessage }).ToList()
            ),
            BadRequestException badRequestEx => (
                HttpStatusCode.BadRequest,
                new List<MessageTemplate> { new() { MessageKey = badRequestEx.Message, Args = badRequestEx.Details != null ? new[] { badRequestEx.Details } : null } }
            ),
            NotFoundException notFoundEx => (
                HttpStatusCode.NotFound,
                new List<MessageTemplate> { new() { MessageKey = "RecordNotFound", Args = new[] { notFoundEx.Message } } }
            ),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                new List<MessageTemplate> { new() { MessageKey = "Unauthorized" } }
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                new List<MessageTemplate> { new() { MessageKey = "UnexpectedError" } }
            )
        };

        httpContext.Response.StatusCode = (int)statusCode;
        httpContext.Response.ContentType = "application/json";

        var response = new ApiResponse
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Errors = errors
        };

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken: cancellationToken);
        return true;
    }
}