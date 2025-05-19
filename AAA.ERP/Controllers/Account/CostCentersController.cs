using ERP.Domain.Commands.Account.CostCenters;
using ERP.Domain.Models.Entities.Account.CostCenters;

namespace ERP.API.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
public class CostCentersController : BaseTreeSettingController<CostCenter, CostCenterCreateCommand, CostCenterUpdateCommand>
{
    public CostCentersController(ICostCenterService service,
        IStringLocalizer<Resource> localizer,
        ISender sender) : base(service, localizer, sender)
    { }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] CostCenterCreateCommand input)
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
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] CostCenterUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}