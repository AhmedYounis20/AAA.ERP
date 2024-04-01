using AAA.ERP.InputModels;
using AAA.ERP.Models.Entities.AccountGuide;
using AAA.ERP.Resources;
using AAA.ERP.Services.Interfaces;
using AAA.ERP.Validators.InputValidators;
using AutoMapper;
using Microsoft.Extensions.Localization;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountGuidesController : BaseSettingController<AccountGuide, AccountGuideInputModel>
{
    public AccountGuidesController(IAccountGuideService service,
        AccountGuideInputValidator validator,
        IStringLocalizer<Resource> localizer,
        IMapper mapper) : base(service, validator, localizer, mapper)
    { }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] AccountGuideInputModel input)
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
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] AccountGuideInputModel input)
    {
        return await UpdateRecord(id, input);
    }
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }

}