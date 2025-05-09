using AAA.ERP.Controllers.BaseControllers;
using Domain.Account.Commands.Entries.JournalEntries;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.Interfaces.Entries;
using MediatR;
using Microsoft.Extensions.Localization;
using Shared.Resources;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JournalEntriesController : BaseController<Entry, JournalEntryCreateCommand, JournalEntryUpdateCommand>
{
    private IJournalEntryService _service;

    public JournalEntriesController(IJournalEntryService service,
        IStringLocalizer<Resource> localizer,
        ISender sender) : base(service, localizer, sender)
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
        return StatusCode((int) result.StatusCode, result);
    }
    
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id,JournalEntryUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }
    
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }

    [HttpGet("GetEntryNumber")]
    public async Task<IActionResult> GetEntryNumber([FromQuery]DateTime dateTime)
    {
        var result = await _service.GetEntryNumber(dateTime);
        return StatusCode((int)result.StatusCode, result);
    }
}