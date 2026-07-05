using Inventory.Application.Commands.SellingPrices;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.SellingPrices;
using Inventory.Application.Dtos.Lookups;
using ERP.BuildingBlocks.Application.DTOs;

namespace Inventory.Api.Controllers.Pricing;

/// <summary>CRUD API for selling price settings and lookup queries.</summary>
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

    /// <summary>Creates a selling price.</summary>
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] SellingPriceCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>Lists all selling prices as DTOs.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetDtos();
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Lists selling prices with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Gets a selling price by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    /// <summary>Returns cached selling-price lookup rows for dropdowns.</summary>
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

    /// <summary>Updates a selling price.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] SellingPriceUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes a selling price.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}
