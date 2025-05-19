using ERP.Application.Validators.Account.InputValidators;
using ERP.Domain.Commands.Account.Currencies;
using ERP.Domain.Models.Entities.Account.Currencies;

namespace ERP.API.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
public class CurrenciesController : BaseSettingController<Currency, CurrencyCreateCommand, CurrencyUpdateCommand>
{
    public CurrenciesController(ICurrencyService service,
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