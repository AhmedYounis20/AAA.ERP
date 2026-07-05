using Accounting.Application.Commands.Roles;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Roles;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Api.Controllers.Settings;

/// <summary>CRUD API for job role settings (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RolesController(ISender sender, IRoleReadService reads) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoleCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    [HttpGet]
    public async Task<IActionResult> Get() => this.ToResult(await reads.GetAll());

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await reads.GetPaginated(filter, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id) => this.ToResult(await reads.GetById(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RoleUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => this.ToResult(await sender.Send(new RoleDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);
}
