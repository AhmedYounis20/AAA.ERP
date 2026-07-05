using Inventory.Application.Commands.Sizes;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Sizes;
using Shared.DTOs;

namespace ERP.Host.Controllers.Inventory;

/// <summary>CRUD API for inventory size settings.</summary>
[Route("api/[controller]")]
[ApiController]
public class SizesController : BaseSettingController<Size, SizeCreateCommand, SizeUpdateCommand>
{
    private readonly ISizeService _service;
    public SizesController(ISizeService service,
        ISender mapper) : base(service, mapper)
    {
        _service = service;
    }

    /// <summary>Creates a size.</summary>
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] SizeCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>Lists all sizes.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    /// <summary>Lists sizes with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Gets a size by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    /// <summary>Updates a size.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] SizeUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes a size.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }

    /// <summary>Returns the next available size code.</summary>
    [HttpGet("nextCode")]
    public async Task<IActionResult> GetNextCode()
    {
        var result = await _service.GetNextCodeAsync();
        return StatusCode((int)result.StatusCode, result);
    }
}
