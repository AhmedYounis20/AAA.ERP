using System.Net;
using ERP.BuildingBlocks.Application.Responses;

namespace ERP.BuildingBlocks.Application.CQRS.Handlers.Execution;

internal static class CrudResponseFactory
{
    public static ApiResponse<TEntity> Created<TEntity>(TEntity entity) =>
        new()
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Result = entity
        };

    public static ApiResponse<TEntity> Ok<TEntity>(TEntity? entity) =>
        new()
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = entity
        };

    public static ApiResponse<TEntity> BadRequest<TEntity>(IEnumerable<string> errorKeys) =>
        new()
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.BadRequest,
            Errors = errorKeys.Select(e => new MessageTemplate { MessageKey = e }).ToList()
        };

    public static ApiResponse<TEntity> FromException<TEntity>(Exception ex) =>
        new()
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.BadRequest,
            Errors = new List<MessageTemplate> { new() { MessageKey = ex.Message } }
        };
}
