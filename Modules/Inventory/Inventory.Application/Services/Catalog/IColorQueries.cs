using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.Queries;
using Inventory.Domain.Models.Entities.Colors;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for color GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IColorQueries : ISettingQueries<Color>
{
    Task<ApiResponse<string>> GetNextCode();
}
