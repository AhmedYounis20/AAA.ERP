using AAA.ERP.Controllers.BaseControllers;
using Domain.Account.Commands.FinancialPeriods;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Shared.Resources;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class
    FinancialPeriodsController : BaseController<FinancialPeriod, FinancialPeriodCreateCommand,
    FinancialPeriodUpdateCommand>
{
    IFinancialPeriodService _service;

    public FinancialPeriodsController(IFinancialPeriodService service,
        IStringLocalizer<Resource> localizer,
        ISender sender)
        : base(service, localizer, sender)
    {
        _service = service;
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] FinancialPeriodCreateCommand input)
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

    [HttpGet("GetCurrentFinancialPeriod")]
    public virtual async Task<IActionResult> GetCurrentFinancialPeriod()
    {
        var result = await _service.GetCurrentFinancailPeriod();
        return StatusCode((int) result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] FinancialPeriodUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}