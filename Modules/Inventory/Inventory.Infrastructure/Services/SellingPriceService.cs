using Inventory.Domain.Commands.SellingPrices;
using Inventory.Domain.Models.Entities.SellingPrices;

namespace Inventory.Infrastructure.Services;

public class SellingPriceService :
    BaseSettingService<SellingPrice, SellingPriceCreateCommand, SellingPriceUpdateCommand>, ISellingPriceService
{
    ISellingPriceRepository _repository;
    public SellingPriceService(ISellingPriceRepository repository) : base(repository)
    => _repository = repository;
    


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
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }
}
