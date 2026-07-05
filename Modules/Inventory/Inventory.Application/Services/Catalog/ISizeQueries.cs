using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.Queries;
using Inventory.Domain.Models.Entities.Sizes;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for size GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ISizeQueries : ISettingQueries<Size>
{
    Task<ApiResponse<string>> GetNextCode();
}
