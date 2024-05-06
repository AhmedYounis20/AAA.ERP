using AAA.ERP.InputModels;
using AAA.ERP.Models.Entities.ChartOfAccount;
using AAA.ERP.Resources;
using AAA.ERP.Services.Interfaces;
using AAA.ERP.Validators.InputValidators;
using AutoMapper;
using Microsoft.Extensions.Localization;

namespace AAA.ERP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChartOfAccountsController : BaseTreeSettingController<ChartOfAccount, ChartOfAccountInputModel>
{
    IChartOfAccountService _service;
    public ChartOfAccountsController(IChartOfAccountService service,
        ChartOfAccountInputValidator validator,
        IStringLocalizer<Resource> localizer,
        IMapper mapper) : base(service, validator, localizer, mapper)
    => _service = service;

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] ChartOfAccountInputModel input)
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
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] ChartOfAccountInputModel input)
    {
        return await UpdateRecord(id, input);
    }
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
    [HttpGet("generateNewChildCode")]
    public async Task<IActionResult> GenerateNewChildCode([FromQuery] Guid? parentId)
    {
        return Ok(await _service.GenerateNewCodeForChild(parentId));
    }

    [HttpGet("NextAccountDefaultData")]
    public async Task<IActionResult> NextAccountDefaultData([FromQuery] Guid? parentId)
    {
        return Ok(await _service.NextAccountDefaultData(parentId));
    }

}