using AAA.ERP.Controllers.BaseControllers;
using AutoMapper;
using Domain.Account.Commands.Currencies;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Services.BaseServices.interfaces;
using Domain.Account.Validators.InputValidators;
using MediatR;
using Microsoft.Extensions.Localization;
using Shared.Resources;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrenciesController : BaseSettingController<Currency, CurrencyCreateCommand,CurrencyUpdateCommand>
{
    public CurrenciesController(IBaseSettingService<Currency> service,
        CurrencyInputValidator validator, 
        IStringLocalizer<Resource> localizer,
        ISender sender) : base(service, localizer, sender)
    { }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] CurrencyCreateCommand input)
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
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] CurrencyUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}