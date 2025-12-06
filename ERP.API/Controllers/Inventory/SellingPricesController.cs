using ERP.Application.Services.Account;
using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.SellingPrices;
using ERP.Domain.Models.Entities.Inventory.SellingPrices;
using ERP.Domain.OutputDtos.Lookups;

namespace ERP.API.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class SellingPricesController : BaseSettingController<SellingPrice, SellingPriceCreateCommand, SellingPriceUpdateCommand>
{
    private readonly ISellingPriceService _service;
    IBaseQueryService<SellingPrice, SellingPriceLookupDto> _baseQueryService;

    public SellingPricesController(ISellingPriceService service,
        IBaseQueryService<SellingPrice, SellingPriceLookupDto> baseQueryService,
        ISender mapper) : base(service, mapper)
    {
        _service = service;
        _baseQueryService = baseQueryService;
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] SellingPriceCreateCommand input)
    {
        return await CreateRecord(input);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetDtos();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    [HttpGet("lookups")]
    public virtual async Task<IActionResult> GetLookUps()
    {
        var result = new ApiResponse<IEnumerable<SellingPriceLookupDto>>();
        try
        {
            result = new ApiResponse<IEnumerable<SellingPriceLookupDto>>
            {
                Result = await _baseQueryService.GetLookUps(),
                IsSuccess = true
            };
        }
        catch
        {
            result = new ApiResponse<IEnumerable<SellingPriceLookupDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] SellingPriceUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}