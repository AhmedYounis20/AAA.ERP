using AAA.ERP.InputModels;
using AAA.ERP.Models.Entities.Currencies;
using AAA.ERP.Resources;
using AAA.ERP.Services.BaseServices.interfaces;
using AAA.ERP.Validators.InputValidators;
using AutoMapper;
using Microsoft.Extensions.Localization;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrenciesController : BaseSettingController<Currency, CurrencyInputModel>
{
    public CurrenciesController(IBaseSettingService<Currency> service, CurrencyValidator validator, IStringLocalizer<Resource> localizer, IMapper mapper) : base(service, validator, localizer, mapper)
    { }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] CurrencyInputModel input)
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
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] CurrencyInputModel input)
    {
        return await UpdateRecord(id, input);
    }
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}