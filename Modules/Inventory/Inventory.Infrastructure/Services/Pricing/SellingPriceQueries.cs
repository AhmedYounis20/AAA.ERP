using System.Net;
using ERP.BuildingBlocks.Application.Responses;
using Inventory.Application.Commands.SellingPrices;
using Inventory.Application.Dtos;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.SellingPrices;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Inventory.Infrastructure.Services;

/// <summary>
/// Selling price read queries.
/// </summary>
public class SellingPriceQueries(ISellingPriceRepository repository)
    : BaseSettingRepositoryQueries<SellingPrice>(repository),
      ISellingPriceQueries
{
    public async Task<ApiResponse<IEnumerable<SellingPriceDto>>> GetDtos()
    {
        try
        {
            var entities = await repository.GetDtos();
            return new ApiResponse<IEnumerable<SellingPriceDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<SellingPriceDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }
}
