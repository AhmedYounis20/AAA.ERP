using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Sizes;
using ERP.Domain.Models.Entities.Inventory.Sizes;

namespace ERP.API.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class SizesController : BaseSettingController<Size, SizeCreateCommand, SizeUpdateCommand>
{
    private readonly ISizeService _service;
    public SizesController(ISizeService service,
        IStringLocalizer<Resource> localizer,
        ISender mapper) : base(service, localizer, mapper)
    {
        _service = service;
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] SizeCreateCommand input)
    {
        return await CreateRecord(input);
    }
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] SizeUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
    [HttpGet("nextCode")]
    public async Task<IActionResult> GetNextCode()
    {
        var result = await _service.GetNextCodeAsync();
        return StatusCode((int)result.StatusCode, result);
    }
}