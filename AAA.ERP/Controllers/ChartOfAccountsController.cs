using Domain.Account.Commands.ChartOfAccounts;
using Domain.Account.Models.Entities.ChartOfAccounts;

namespace ERP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChartOfAccountsController : BaseTreeSettingController<ChartOfAccount, ChartOfAccountCreateCommand,ChartOfAccountUpdateCommand>
{
    IChartOfAccountService _service;
    public ChartOfAccountsController(IChartOfAccountService service,
        IStringLocalizer<Resource> localizer,
        ISender sender) : base(service, localizer, sender)
    => _service = service;

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] ChartOfAccountCreateCommand input)
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
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] ChartOfAccountUpdateCommand input)
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