using Inventory.Application.Commands.SellingPrices;
using Inventory.Application.Repositories;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.SellingPrices;
using ERP.Infrastructure.Services.BaseServices;

namespace Inventory.Infrastructure.Services;

public class SellingPriceService
    : BaseSettingService<SellingPrice, SellingPriceCreateCommand, SellingPriceUpdateCommand>, ISellingPriceService
{
    private readonly ISellingPriceRepository _repository;

    public SellingPriceService(ISellingPriceRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<SellingPriceDto>>> GetDtos()
    {
        try
        {
            var entities = await _repository.GetDtos();
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
