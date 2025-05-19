using ERP.API.Controllers.BaseControllers;
using ERP.Application.Services.Account.Entries;
using ERP.Domain.Commands.Account.Entries;
using ERP.Domain.Models.Entities.Account.Entries;

namespace ERP.API.Controllers.Account.Entries;

[Route("api/[controller]")]
[ApiController]
public class EntriesController : BaseController<Entry, ComplexEntryCreateCommand, ComplexEntryUpdateCommand>
{
    private IComplexEntryService _service;

    public EntriesController(IComplexEntryService service,
        IStringLocalizer<Resource> localizer,
        ISender sender) : base(service, localizer, sender)
        => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ComplexEntryCreateCommand input)
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
        var result = await _service.GetComplexEntryById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, ComplexEntryUpdateCommand input)
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