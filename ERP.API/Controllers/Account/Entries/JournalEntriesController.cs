using ERP.API.Controllers.BaseControllers;
using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries.JournalEntries;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.API.Controllers.Account.Entries;

[Route("api/[controller]")]
[ApiController]
public class JournalEntriesController : BaseController<Entry, JournalEntryCreateCommand, JournalEntryUpdateCommand>
{
    private IJournalEntryService _service;

    public JournalEntriesController(IJournalEntryService service,
        ISender sender) : base(service, sender)
        => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] JournalEntryCreateCommand input)
    {
        return await CreateRecord(input);
    }
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        var result = await _service.GetDto();
        return StatusCode((int)result.StatusCode, result);
    }
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        var result = await _service.GetDto(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, JournalEntryUpdateCommand input)
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