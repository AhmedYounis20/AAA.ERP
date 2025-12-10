using ERP.Application.Services.Inventory;
using ERP.Domain.Commands.Inventory.Colors;
using ERP.Domain.Models.Entities.Inventory.Colors;
using Shared.DTOs;

namespace ERP.API.Controllers.Inventory;

[Route("api/[controller]")]
[ApiController]
public class ColorsController : BaseSettingController<Color, ColorCreateCommand, ColorUpdateCommand>
{
    private readonly IColorService _service;
    public ColorsController(IColorService service,
        ISender mapper) : base(service, mapper)
    {
        _service = service;
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] ColorCreateCommand input)
    {
        return await CreateRecord(input);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] ColorUpdateCommand input)
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
