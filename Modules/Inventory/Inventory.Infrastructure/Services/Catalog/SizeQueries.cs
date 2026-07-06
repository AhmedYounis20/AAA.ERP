using System.Net;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Sizes;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Size read queries.
/// </summary>
public class SizeQueries(ISizeRepository repository)
    : BaseSettingRepositoryQueries<Size>(repository),
      ISizeQueries
{
    public async Task<ApiResponse<string>> GetNextCode()
    {
        try
        {
            var maxCode = await repository.GetMaxCodeAsync();
            var next = 1;
            if (int.TryParse(maxCode, out var max))
                next = max + 1;

            return new ApiResponse<string>
            {
                Result = next.ToString(),
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }
}
