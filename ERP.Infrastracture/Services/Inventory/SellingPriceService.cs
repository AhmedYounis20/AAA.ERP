using ERP.Application.Repositories.Inventory;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.SellingPrices;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;

namespace ERP.Infrastracture.Services.Inventory;

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