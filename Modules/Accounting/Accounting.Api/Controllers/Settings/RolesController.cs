using Accounting.Application.Commands.Roles;
using Accounting.Domain.Models.Entities.Roles;

namespace Accounting.Api.Controllers.Settings;

/// <summary>CRUD API for job role settings (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RolesController(ISender sender, IRoleQueries queries)
    : BaseSettingController<Role, RoleCreateCommand, RoleUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<Role>> BuildDeleteCommand(Guid id)
        => new RoleDeleteCommand(id);
}
