using Inventory.Application.Commands.Colors;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Colors;
using Shared.DTOs;

namespace ERP.Host.Controllers.Inventory;

/// <summary>CRUD API for inventory color settings.</summary>
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

    /// <summary>Creates a color.</summary>
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] ColorCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>Lists all colors.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    /// <summary>Lists colors with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Gets a color by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    /// <summary>Updates a color.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] ColorUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes a color.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }

    /// <summary>Returns the next available color code.</summary>
    [HttpGet("nextCode")]
    public async Task<IActionResult> GetNextCode()
    {
        var result = await _service.GetNextCodeAsync();
        return StatusCode((int)result.StatusCode, result);
    }
}
