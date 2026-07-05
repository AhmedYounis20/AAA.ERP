using Accounting.Application.Commands.Roles;
using Accounting.Domain.Models.Entities.Roles;
using ERP.BuildingBlocks.Application.DTOs;

namespace ERP.Host.Controllers.Accounting;

/// <summary>CRUD API for job role settings.</summary>
[Route("api/[controller]")]
[ApiController]
public class RolesController : BaseSettingController<Role, RoleCreateCommand, RoleUpdateCommand>
{
    public RolesController(IRoleService service,
        ISender mapper) : base(service, mapper)
    { }

    /// <summary>Creates a new record.</summary>
    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] RoleCreateCommand input)
    {
        return await CreateRecord(input);
    }

    /// <summary>Lists all records.</summary>
    [HttpGet]
    public virtual async Task<IActionResult> Get()
    {
        return await GetAllRecords();
    }

    /// <summary>Lists records with pagination and filtering.</summary>
    [HttpGet("paginated")]
    public virtual async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
    {
        return await GetAllRecordsPaginated(filter, cancellationToken);
    }

    /// <summary>Gets a record by identifier.</summary>
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> Get(Guid id)
    {
        return await GetRecord(id);
    }

    /// <summary>Updates an existing record.</summary>
    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(Guid id, [FromBody] RoleUpdateCommand input)
    {
        return await UpdateRecord(id, input);
    }

    /// <summary>Deletes a record by identifier.</summary>
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync(Guid id)
    {
        return await DeleteRecord(id);
    }
}
