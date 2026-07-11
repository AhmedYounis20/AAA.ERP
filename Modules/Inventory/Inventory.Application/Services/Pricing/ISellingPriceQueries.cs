using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.Queries;
using Inventory.Application.Dtos;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Application.Services;

/// <summary>
/// Read-side contract for selling price GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface ISellingPriceQueries : ISettingQueries<SellingPrice>
{
    Task<ApiResponse<IEnumerable<SellingPriceDto>>> GetDtos();
}
