using ERP.Domain.Commands.Account.AccountGuides;
using ERP.Domain.Models.Entities.Account.AccountGuides;

namespace ERP.API.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
public class AccountGuidesController : BaseSettingController<AccountGuide, AccountGuideCreateCommand, AccountGuideUpdateCommand>
{
    public AccountGuidesController(IAccountGuideService service,
        IStringLocalizer<Resource> localizer,
        ISender mapper) : base(service, localizer, mapper)
    { }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] AccountGuideCreateCommand input)
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
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] AccountGuideUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }

}