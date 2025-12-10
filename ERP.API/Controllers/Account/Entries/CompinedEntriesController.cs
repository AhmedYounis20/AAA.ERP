using ERP.API.Controllers.BaseControllers;
using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries.CompinedEntries;
using ERP.Domain.Models.Entities.Account.Entries;
using Shared.DTOs.Filters;

namespace ERP.API.Controllers.Account.Entries;

[Route("api/[controller]")]
[ApiController]
public class CompinedEntriesController : BaseController<Entry, CompinedEntryCreateCommand, CompinedEntryUpdateCommand>
{
    private ICompinedEntryService _service;

    public CompinedEntriesController(ICompinedEntryService service,
        ISender sender) : base(service, sender)
        => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CompinedEntryCreateCommand input)
    {
        return await CreateRecord(input);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetComplexEntries();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] EntryFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetComplexEntryById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, CompinedEntryUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }

    [HttpGet("GetEntryNumber")]
    public async Task<IActionResult> GetEntryNumber([FromQuery] DateTime dateTime)
    {
        var result = await _service.GetEntryNumber(dateTime);
        return StatusCode((int)result.StatusCode, result);
    }
}
