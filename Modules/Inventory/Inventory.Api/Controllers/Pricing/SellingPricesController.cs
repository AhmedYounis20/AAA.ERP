using Inventory.Application.Commands.SellingPrices;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.SellingPrices;
using Inventory.Application.Dtos.Lookups;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;
using Accounting.Application.Services.Queries;

namespace Inventory.Api.Controllers.Pricing;

/// <summary>CRUD API for selling price settings and lookup queries (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SellingPricesController(
    ISender sender,
    ISellingPriceReadService reads,
    IBaseQueryService<SellingPrice, SellingPriceLookupDto> lookups) : ControllerBase
{
    /// <summary>Creates a selling price.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SellingPriceCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    /// <summary>Lists all selling prices as DTOs.</summary>
    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await reads.GetDtos());

    /// <summary>Lists selling prices with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await reads.GetPaginated(filter, cancellationToken));

    /// <summary>Gets a selling price by identifier.</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => this.ToResult(await reads.GetById(id));

    /// <summary>Returns cached selling-price lookup rows for dropdowns.</summary>
    [HttpGet("lookups")]
    public async Task<IActionResult> GetLookUps()
    {
        try
        {
            return this.ToResult(new ApiResponse<IEnumerable<SellingPriceLookupDto>>
            {
                Result = await lookups.GetLookUps(),
                IsSuccess = true
            });
        }
        catch
        {
            return this.ToResult(new ApiResponse<IEnumerable<SellingPriceLookupDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            });
        }
    }

    /// <summary>Updates a selling price.</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SellingPriceUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    /// <summary>Deletes a selling price.</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => this.ToResult(await sender.Send(new SellingPriceDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);
}
