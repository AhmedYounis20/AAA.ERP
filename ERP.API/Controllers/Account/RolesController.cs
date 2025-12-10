using ERP.Domain.Commands.Account.Roles;
using ERP.Domain.Models.Entities.Account.Roles;
using Shared.DTOs;

namespace ERP.API.Controllers.Account;

[Route("api/[controller]")]
[ApiController]
public class RolesController : BaseSettingController<Role, RoleCreateCommand, RoleUpdateCommand>
{
    public RolesController(IRoleService service,
        ISender mapper) : base(service, mapper)
    { }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] RoleCreateCommand input)
    {
        return await CreateRecord(input);
    }

    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] RoleUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}
