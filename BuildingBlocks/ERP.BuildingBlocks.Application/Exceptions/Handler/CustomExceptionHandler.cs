using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.Responses;
using System.Net;

namespace ERP.BuildingBlocks.Application.Exceptions.Handler;

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
                new List<MessageTemplate> { new() { MessageKey = ErrorMessages.InternalServerError, Args = new[] { internalEx.Details ?? internalEx.Message } } }
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
                new List<MessageTemplate> { new() { MessageKey = ErrorMessages.RecordNotFound, Args = new[] { notFoundEx.Message } } }
            ),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                new List<MessageTemplate> { new() { MessageKey = ErrorMessages.Unauthorized } }
            ),
            ForbiddenException forbiddenEx => (
                HttpStatusCode.Forbidden,
                new List<MessageTemplate> { new() { MessageKey = forbiddenEx.Message } }
            ),
            ConflictException conflictEx => (
                HttpStatusCode.Conflict,
                new List<MessageTemplate> { new() { MessageKey = conflictEx.Message } }
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                new List<MessageTemplate> { new() { MessageKey = ErrorMessages.UnexpectedError } }
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
