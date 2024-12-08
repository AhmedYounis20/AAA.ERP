using AAA.ERP.Controllers.BaseControllers;
using AutoMapper;
using Domain.Account.Commands.Currencies;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Models.Entities.Entries;
using Domain.Account.Services.BaseServices.interfaces;
using Domain.Account.Services.Interfaces;
using Domain.Account.Validators.InputValidators;
using MediatR;
using Microsoft.Extensions.Localization;
using Shared.Resources;
using Shared.Responses;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EntriesController : BaseController<Entry, EntryCreateCommand,EntryUpdateCommand>
{
    private IEntryService _service;

    public EntriesController(IEntryService service,
        IStringLocalizer<Resource> localizer,
        ISender sender) : base(service, localizer, sender)
        => _service = service;

    [HttpPost]
    public virtual async Task<IActionResult> Create(EntryCreateCommand input)
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
    public virtual async Task<IActionResult> Update(Guid id,EntryUpdateCommand input)
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