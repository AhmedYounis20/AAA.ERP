using Accounting.Application.Commands.Entries;
using Accounting.Domain.Models.Entities.Entries;
using Accounting.Application.Dtos.Filters;

namespace Accounting.Api.Controllers.Entries;

/// <summary>CRUD API for complex accounting entries.</summary>
[Route("api/[controller]")]
[ApiController]
public class EntriesController : BaseController<Entry, ComplexEntryCreateCommand, ComplexEntryUpdateCommand>
{
    private IComplexEntryService _service;

    public EntriesController(IComplexEntryService service,
        ISender sender) : base(service, sender)
        => _service = service;

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ComplexEntryCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>Lists all records.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    /// <summary>Lists records with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] EntryFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Gets a record by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetComplexEntryById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    /// <summary>Updates an existing record.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, ComplexEntryUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes a record by identifier.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }

    /// <summary>Returns the next entry number for a given date.</summary>
    [HttpGet("GetEntryNumber")]
    public async Task<IActionResult> GetEntryNumber([FromQuery] DateTime dateTime)
    {
        var result = await _service.GetEntryNumber(dateTime);
        return StatusCode((int)result.StatusCode, result);
    }
}
