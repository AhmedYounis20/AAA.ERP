using Inventory.Application.Services;
using Inventory.Domain.Commands.SellingPrices;
using Inventory.Domain.Models.Entities.SellingPrices;
using Shared.API.Controllers;

namespace Inventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SellingPricesController : BaseSettingController<SellingPrice, SellingPriceCreateCommand, SellingPriceUpdateCommand>
{
    private readonly ISellingPriceService _service;
    public SellingPricesController(ISellingPriceService service,
        ISender mapper) : base(service, mapper)
    => _service = service;

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] SellingPriceCreateCommand input)
    {
        return await CreateRecord(input);
    }
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetDtos();
        return StatusCode((int)result.StatusCode,result);
    }
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
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
